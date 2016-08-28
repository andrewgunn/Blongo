namespace Blongo.Autofac
{
    using global::Autofac;
    using Microsoft.Extensions.WebEncoders;

    public class HtmlModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HtmlEncoder>()
                .As<IHtmlEncoder>()
                .AsSelf();
        }
    }
}
