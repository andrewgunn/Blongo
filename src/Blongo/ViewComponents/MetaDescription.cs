namespace Blongo.ViewComponents
{
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Models.MetaDescription;
    using MongoDB.Driver;
    using Blog = Data.Blog;

    public class MetaDescription : ViewComponent
    {
        private readonly MongoClient _mongoClient;

        public MetaDescription(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<Blog>(CollectionNames.Blogs);
            var blog = await collection.Find(Builders<Blog>.Filter.Empty)
                .Project(b => new Models.MetaDescription.Blog(b.Description))
                .SingleOrDefaultAsync();

            var viewModel = new MetaDescriptionViewModel(blog);

            return View(viewModel);
        }
    }
}