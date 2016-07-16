using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Blongo.Filters
{
    public class BlogDataFilter : ActionFilterAttribute
    {
        public BlogDataFilter(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.Blog>(Data.CollectionNames.Blogs);
            var blog = await collection.Find(Builders<Data.Blog>.Filter.Empty)
                .Project(b => new
                {
                    b.Name,
                    b.Description,
                    b.FeedUrl,
                    Author = new
                    {
                        b.Author.Name,
                        b.Author.WebsiteUrl,
                        b.Author.EmailAddress,
                        b.Author.GitHubUsername,
                        b.Author.TwitterUsername
                    }
                })
                .SingleOrDefaultAsync();

            if (blog != null)
            {
                ((dynamic)context.Controller).ViewBag._BlogName = blog.Name;
                ((dynamic)context.Controller).ViewBag._BlogDescription = blog.Description;
                ((dynamic)context.Controller).ViewBag._BlogFeedUrl = blog.FeedUrl;
                ((dynamic)context.Controller).ViewBag._BlogAuthorName = blog.Author.Name;
                ((dynamic)context.Controller).ViewBag._BlogAuthorWebsiteUrl = blog.Author.WebsiteUrl;
                ((dynamic)context.Controller).ViewBag._BlogAuthorEmailAddress = blog.Author.EmailAddress;
                ((dynamic)context.Controller).ViewBag._BlogAuthorGitHubUsername = blog.Author.GitHubUsername;
                ((dynamic)context.Controller).ViewBag._BlogAuthorTwitterUsername = blog.Author.TwitterUsername;
            }

            await next();
        }

        private readonly MongoClient _mongoClient;
    }
}
