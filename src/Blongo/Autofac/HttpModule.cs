namespace Blongo.Autofac
{
    using System.Net.Http;
    using global::Autofac;

    public class HttpModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpClient>()
                .AsSelf();
        }
    }
}
