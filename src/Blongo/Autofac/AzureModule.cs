namespace Blongo.Autofac
{
    using Data;
    using global::Autofac;
    using MongoDB.Driver;

    public class AzureModule : Module
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
                        b.AzureStorageConnectionString
                    })
                    .SingleOrDefaultAsync()
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();

                return new AzureBlobStorage(blog?.AzureStorageConnectionString);
            });
        }
    }
}