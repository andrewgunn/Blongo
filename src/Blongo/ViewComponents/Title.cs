namespace Blongo.ViewComponents
{
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Models.Title;
    using MongoDB.Driver;
    using Blog = Data.Blog;

    public class Title : ViewComponent
    {
        private readonly MongoClient _mongoClient;

        public Title(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<Blog>(CollectionNames.Blogs);
            var blog = await collection.Find(Builders<Blog>.Filter.Empty)
                .Project(b => new Models.Title.Blog(b.Name))
                .SingleOrDefaultAsync();

            var viewModel = new TitleViewModel(blog);

            return View(viewModel);
        }
    }
}