using Blongo.Models.Sidebar;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Blongo.ViewComponents
{
    public class Sidebar : ViewComponent
    {
        public Sidebar(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.Blog>(Data.CollectionNames.Blogs);
            var blog = await collection.Find(Builders<Data.Blog>.Filter.Empty)
                .Project(b => new Blog(b.Name, b.Description, b.FeedUrl, new Author(b.Author.Name, b.Author.WebsiteUrl, b.Author.EmailAddress, b.Author.GitHubUsername, b.Author.TwitterUsername)))
                .SingleOrDefaultAsync();

            var viewModel = new SidebarViewModel(blog);

            return View(viewModel);
        }

        private readonly MongoClient _mongoClient;
    }
}
