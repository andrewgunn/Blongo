namespace Blongo.ViewComponents
{
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Models.FeedLink;
    using MongoDB.Driver;
    using Blog = Data.Blog;

    public class FeedLink : ViewComponent
    {
        private readonly MongoClient _mongoClient;

        public FeedLink(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<Blog>(CollectionNames.Blogs);
            var blog = await collection.Find(Builders<Blog>.Filter.Empty)
                .Project(b => new Models.FeedLink.Blog(b.Name, b.FeedUrl))
                .SingleOrDefaultAsync();

            var viewModel = new FeedLinkViewModel(blog);

            return View(viewModel);
        }
    }
}