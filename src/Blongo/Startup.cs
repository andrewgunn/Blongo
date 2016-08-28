namespace Blongo
{
    using System;
    using System.Net.Http;
    using System.Reflection;
    using Configuration;
    using Data;
    using global::Autofac;
    using global::Autofac.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.WebEncoders;
    using ModelBinding;
    using MongoDB.Driver;
    using Routing;
    using Serilog;

    public class Startup
    {
        public Startup(IHostingEnvironment hostingEnvironment)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables()
                .AddApplicationInsightsSettings(hostingEnvironment.IsDevelopment());

            Configuration = configurationBuilder.Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.ApplicationInsightsEvents(
                    Configuration.GetValue<string>("ApplicationInsights:InstrumentationKey"))
                .CreateLogger();

            MongoDbConfig.RegisterCamelCaseElementNameConvention();

            Password.ConstantSalt = Configuration.GetValue<string>("Blongo:PasswordConstantSalt");
        }

        public IConfigurationRoot Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc(options => { options.ModelBinderProviders.Add(new ObjectIdModelBinderProvider()); });

            services.AddOptions()
                .Configure<BlongoConfiguration>(Configuration.GetSection("Blongo"))
                .AddRouting(options =>
                {
                    options.AppendTrailingSlash = false;
                    options.ConstraintMap.Add("ObjectId", typeof(ObjectIdRouteConstraint));
                    options.LowercaseUrls = true;
                });

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());
            containerBuilder.Populate(services);
            var container = containerBuilder.Build();
            var serviceProvider = new AutofacServiceProvider(container);

            return serviceProvider;
        }

        public void Configure(IApplicationBuilder applicationBuilder, IHostingEnvironment hostingEnvironment,
            ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddSerilog();

            applicationBuilder.UseApplicationInsightsRequestTelemetry();

            if (hostingEnvironment.IsDevelopment())
            {
                applicationBuilder.UseDeveloperExceptionPage();
            }
            else
            {
                applicationBuilder.UseExceptionHandler("/500");
            }

            applicationBuilder
                .UseStatusCodePagesWithReExecute("/{0}")
                .UseCookieAuthentication(new CookieAuthenticationOptions
                {
                    AutomaticChallenge = true,
                    AuthenticationScheme = "Cookies",
                    AutomaticAuthenticate = true,
                    ExpireTimeSpan = TimeSpan.FromMinutes(30),
                    LoginPath = "/admin/login",
                    ReturnUrlParameter = "returnUrl",
                    SlidingExpiration = true
                })
                .Use(async (context, next) =>
                {
                    if (context.User.Identity.IsAuthenticated)
                    {
                        var mongoClient = context.RequestServices.GetRequiredService<MongoClient>();
                        var database = mongoClient.GetDatabase(DatabaseNames.Blongo);
                        var collection = database.GetCollection<User>(CollectionNames.Users);
                        var userCount =
                            await
                                collection.CountAsync(
                                    Builders<User>.Filter.Where(u => u.EmailAddress == context.User.Identity.Name));

                        if (userCount == 0)
                        {
                            await context.Authentication.SignOutAsync("Cookies");

                            var redirectUrl =
                                $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";

                            context.Response.Redirect(redirectUrl);

                            return;
                        }
                    }

                    await next();
                })
                .UseMvc()
                .UseStaticFiles()
                .UseApplicationInsightsExceptionTelemetry();
        }
    }
}