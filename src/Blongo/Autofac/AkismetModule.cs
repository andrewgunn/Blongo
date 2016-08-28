namespace Blongo.Autofac
{
    using AkismetSdk;
    using AkismetSdk.Clients.CommentCheck;
    using AkismetSdk.Clients.SubmitHam;
    using AkismetSdk.Clients.SubmitSpam;
    using Data;
    using global::Autofac;
    using MongoDB.Driver;

    public class AkismetModule : Module
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
                        b.AkismetApiKey
                    })
                    .SingleOrDefaultAsync()
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();

                return new AkismetSettings(blog?.AkismetApiKey);
            });
            builder.RegisterType<CommentCheckClient>()
                .AsSelf()
                .SingleInstance();
            builder.RegisterType<SubmitSpamClient>()
                .AsSelf()
                .SingleInstance();
            builder.RegisterType<SubmitHamClient>()
                .AsSelf()
                .SingleInstance();
        }
    }
}
