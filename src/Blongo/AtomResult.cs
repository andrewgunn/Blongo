namespace Blongo
{
    using System.ServiceModel.Syndication;
    using System.Text;
    using System.Xml;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    public sealed class AtomResult : ActionResult
    {
        private readonly SyndicationFeed _syndicationFeed;

        public AtomResult(SyndicationFeed syndicationFeed)
        {
            _syndicationFeed = syndicationFeed;
        }

        public override void ExecuteResult(ActionContext context)
        {
            context.HttpContext.Response.ContentType = "application/atom+xml";

            var atom10FeedFormatter = new Atom10FeedFormatter(_syndicationFeed);
            var xmlWriterSettings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8
            };

            var hostingEnvironment =
                (IHostingEnvironment) context.HttpContext.RequestServices.GetService(typeof(IHostingEnvironment));

            if (hostingEnvironment.IsDevelopment())
            {
                xmlWriterSettings.Indent = true;
            }

            using (var xmlWriter = XmlWriter.Create(context.HttpContext.Response.Body, xmlWriterSettings))
            {
                atom10FeedFormatter.WriteTo(xmlWriter);
            }
        }
    }
}