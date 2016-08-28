namespace Blongo.ViewComponents
{
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Models.Sidebar;
    using MongoDB.Driver;
    using Blog = Data.Blog;

    public class Sidebar : ViewComponent
    {
        private readonly MongoClient _mongoClient;

        public Sidebar(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<Blog>(CollectionNames.Blogs);
            var blog = await collection.Find(Builders<Blog>.Filter.Empty)
                .Project(
                    b =>
                        new Models.Sidebar.Blog(b.Name, b.Description,
                            b.Company == null ? null : new Models.Sidebar.Company(b.Company.Name, b.Company.WebsiteUrl),
                            b.FeedUrl,
                            b.Author == null
                                ? null
                                : new Models.Sidebar.Author(b.Author.Name, b.Author.WebsiteUrl, b.Author.EmailAddress,
                                    b.Author.GitHubUsername, b.Author.TwitterUsername)))
                .SingleOrDefaultAsync();

            var viewModel = new SidebarViewModel(blog);

            return View(viewModel);
        }
    }
}