namespace Blongo.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using MongoDB.Driver;

    [ResponseCache(Duration = 86400)]
    [Route("sitemap.xml", Name = "Sitemap")]
    public class SitemapController : Controller
    {
        private readonly MongoClient _mongoClient;

        public SitemapController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public async Task<ContentResult> Index()
        {
            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<Post>(CollectionNames.Posts);
            var filter = Builders<Post>.Filter.Where(p => p.IsPublished && p.PublishedAt <= DateTime.UtcNow);
            var blog = await collection.Find(filter)
                .Sort(Builders<Post>.Sort.Descending(p => p.PublishedAt))
                .Limit(1)
                .Project(p => new {p.LastUpdatedAt})
                .SingleOrDefaultAsync();

            XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var root = new XElement(xmlns + "urlset");

            var sitemapUrls = new List<SitemapUrl>();
            sitemapUrls.Add(
                new SitemapUrl
                {
                    Url = Url.RouteUrl("ListPosts", null, Request.Scheme),
                    LastModifiedAt = blog == null ? null : blog.LastUpdatedAt,
                    ChangeFrequency = SitemapChangeFrequency.Weekly,
                    Priority = 1
                });

            foreach (var sitemapUrl in sitemapUrls)
            {
                var sitemapUrlElement = new XElement(
                    xmlns + "url",
                    new XElement(
                        xmlns + "loc",
                        Uri.EscapeUriString(sitemapUrl.Url)
                        ),
                    sitemapUrl.LastModifiedAt == null
                        ? null
                        : new XElement(
                            xmlns + "lastmod",
                            sitemapUrl.LastModifiedAt.Value.ToString("yyyy-MM-ddTHH:mm:sszzz")
                            ),
                    sitemapUrl.ChangeFrequency == null
                        ? null
                        : new XElement(
                            xmlns + "changefreq",
                            sitemapUrl.ChangeFrequency.Value.ToString().ToLowerInvariant()
                            ),
                    sitemapUrl.Priority == null
                        ? null
                        : new XElement(
                            xmlns + "priority",
                            sitemapUrl.Priority.Value.ToString("F1", CultureInfo.InvariantCulture)
                            )
                    );
                root.Add(sitemapUrlElement);
            }

            var sitemap = new XDocument(root);

            return Content(sitemap.ToString(), "text/xml", Encoding.UTF8);
        }
    }
}