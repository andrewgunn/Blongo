namespace Blongo.Autofac
{
    using Data;
    using global::Autofac;
    using MongoDB.Driver;
    using RealFaviconGeneratorSdk;

    public class RealFaviconGeneratorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(serviceProvider =>
            {
                var mongoClient = serviceProvider.Resolve<MongoClient>();
                var database = mongoClient.GetDatabase(DatabaseNames.Blongo);
                var blogsCollection = database.GetCollection<Blog>(CollectionNames.Blogs);

                var blog = blogsCollection.Find(Builders<Blog>.Filter.Empty)
                    .Project(b => new
                    {
                        b.RealFaviconGeneratorApiKey
                    })
                    .SingleOrDefaultAsync()
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();

                return new RealFaviconGeneratorSettings(blog?.RealFaviconGeneratorApiKey);
            });
            builder.RegisterType<RealFaviconGenerator>()
                .AsSelf()
                .SingleInstance();
        }
    }
}
