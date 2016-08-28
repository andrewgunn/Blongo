namespace Blongo.Controllers
{
    using System.Text;
    using Microsoft.AspNetCore.Mvc;

    [ResponseCache(Duration = 86400)]
    [Route("robots.txt", Name = "Robots")]
    public class RobotsController : Controller
    {
        public ContentResult Index()
        {
            var content = new StringBuilder();

            var blogUrl = Url.RouteUrl("ListPosts", null, Request.Scheme);
            var sitemapUrl = Url.RouteUrl("Sitemap", null, Request.Scheme);

            content.AppendLine("user-agent: *");
            content.AppendLine("allow: /");
            content.AppendLine($"host: {blogUrl}");
            content.Append($"sitemap: {sitemapUrl}");

            return Content(content.ToString(), "text/plain", Encoding.UTF8);
        }
    }
}