using AkismetSdk;
using AkismetSdk.Clients.CommentCheck;
using AkismetSdk.Clients.SubmitHam;
using AkismetSdk.Clients.SubmitSpam;
using Blongo.Config;
using Blongo.Data;
using Blongo.ModelBinding;
using Blongo.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.WebEncoders;
using MongoDB.Driver;
using RealFaviconGeneratorSdk;
using Serilog;
using System;
using System.Net.Http;

namespace Blongo
{
    public class Startup
    {
        public Startup(IHostingEnvironment hostingEnvironment)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("config.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"config.{hostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);

            configurationBuilder.AddApplicationInsightsSettings(developerMode: hostingEnvironment.IsDevelopment());

            configurationBuilder.AddEnvironmentVariables();

            Configuration = configurationBuilder.Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.ApplicationInsightsEvents(Configuration.GetValue<string>("ApplicationInsights:InstrumentationKey"))
                .CreateLogger();

            MongoDbConfig.RegisterCamelCaseElementNameConvention();

            Password.ConstantSalt = Configuration.GetValue<string>("Blongo:PasswordConstantSalt");
        }

        private void AddAkismet(IServiceCollection services)
        {
            services.AddTransient(serviceProvider =>
            {
                var mongoClient = serviceProvider.GetRequiredService<MongoClient>();
                var database = mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
                var blogsCollection = database.GetCollection<Data.Blog>(Data.CollectionNames.Blogs);

                var blog = blogsCollection.Find(Builders<Data.Blog>.Filter.Empty)
                    .Project(b => new
                    {
                        b.AkismetApiKey
                    })
                    .SingleOrDefaultAsync()
                    .Result;

                return new AkismetSettings(blog?.AkismetApiKey);
            });
            services.AddSingleton<CommentCheckClient>();
            services.AddSingleton<SubmitSpamClient>();
            services.AddSingleton<SubmitHamClient>();
        }

        private void AddAzureBlobStorage(IServiceCollection services)
        {
            services.AddTransient(serviceProvider =>
            {
                var mongoClient = serviceProvider.GetRequiredService<MongoClient>();
                var database = mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
                var blogsCollection = database.GetCollection<Data.Blog>(Data.CollectionNames.Blogs);

                var blog = blogsCollection.Find(Builders<Data.Blog>.Filter.Empty)
                    .Project(b => new
                    {
                        b.AzureStorageConnectionString
                    })
                    .SingleOrDefaultAsync()
                    .Result;

                return new AzureBlobStorage(blog?.AzureStorageConnectionString);
            });
        }

        private void AddRealFaviconGenerator(IServiceCollection services)
        {
            services.AddTransient(serviceProvider =>
            {
                var mongoClient = serviceProvider.GetRequiredService<MongoClient>();
                var database = mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
                var blogsCollection = database.GetCollection<Data.Blog>(Data.CollectionNames.Blogs);

                var blog = blogsCollection.Find(Builders<Data.Blog>.Filter.Empty)
                    .Project(b => new
                    {
                        b.RealFaviconGeneratorApiKey
                    })
                    .SingleOrDefaultAsync()
                    .Result;

                return new RealFaviconGeneratorSettings(blog?.RealFaviconGeneratorApiKey);
            });
            services.AddSingleton<RealFaviconGenerator>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc(options =>
            {
                options.ModelBinderProviders.Add(new ObjectIdModelBinderProvider());
            });

            services.AddRouting(options =>
            {
                options.AppendTrailingSlash = false;
                options.ConstraintMap.Add("ObjectId", typeof(ObjectIdRouteConstraint));
                options.LowercaseUrls = true;
            });

            services
                .AddOptions()
                .AddSingleton<IHtmlEncoder, HtmlEncoder>()
                .AddTransient<HttpClient>()
                .Configure<SendGridConfig>(Configuration.GetSection("SendGrid"));

            var mongoConnectionString = Configuration.GetValue<string>("Blongo:ConnectionString");

            services.AddTransient(serviceProvider => new MongoClient(mongoConnectionString));

            AddAkismet(services);
            AddAzureBlobStorage(services);
            AddRealFaviconGenerator(services);

            services.AddLocalization(options => options.ResourcesPath = "Resources");
        }

        public void Configure(IApplicationBuilder applicationBuilder, IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
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
                        var database = mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
                        var collection = database.GetCollection<Data.User>(Data.CollectionNames.Users);
                        var userCount = await collection.CountAsync(Builders<Data.User>.Filter.Where(u => u.EmailAddress == context.User.Identity.Name));

                        if (userCount == 0)
                        {
                            await context.Authentication.SignOutAsync("Cookies");

                            var redirectUrl = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";

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

        public IConfigurationRoot Configuration { get; }
    }
}