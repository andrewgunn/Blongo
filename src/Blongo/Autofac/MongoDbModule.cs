namespace Blongo.Autofac
{
    using Configuration;
    using global::Autofac;
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;

    public class MongoDbModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(serviceProvider =>
            {
                var blongoConfiguration = serviceProvider.Resolve<IOptions<BlongoConfiguration>>();

                return new MongoClient(blongoConfiguration.Value.ConnectionString);
            });
        }
    }
}
