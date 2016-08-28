namespace Blongo.Autofac
{
    using Data;
    using global::Autofac;
    using MongoDB.Driver;
    using SendGrid;
    using SendGridSettings = Blongo.SendGridSettings;

    public class SendGridModule : Module
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
                        b.SendGridSettings
                    })
                    .SingleOrDefaultAsync()
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();

                return new SendGridSettings(blog?.SendGridSettings?.Username, blog?.SendGridSettings?.Password,
                    blog?.SendGridSettings?.FromEmailAddress);
            });
            builder.RegisterType<SendGridShim>()
                .As<ISendGridShim>()
                .SingleInstance();
        }
    }
}