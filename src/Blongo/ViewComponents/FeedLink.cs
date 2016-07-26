using Blongo.Models.FeedLink;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Blongo.ViewComponents
{
    public class FeedLink : ViewComponent
    {
        public FeedLink(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.Blog>(Data.CollectionNames.Blogs);
            var blog = await collection.Find(Builders<Data.Blog>.Filter.Empty)
                .Project(b => new Blog(b.Name, b.FeedUrl))
                .SingleOrDefaultAsync();

            var viewModel = new FeedLinkViewModel(blog);

            return View(viewModel);
        }

        private readonly MongoClient _mongoClient;
    }
}
