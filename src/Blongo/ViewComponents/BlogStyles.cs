namespace Blongo.ViewComponents
{
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Models.BlogStyles;
    using MongoDB.Driver;

    public class BlogStyles : ViewComponent
    {
        private readonly MongoClient _mongoClient;

        public BlogStyles(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<Blog>(CollectionNames.Blogs);
            var blog = await collection.Find(Builders<Blog>.Filter.Empty)
                .Project(b => new
                {
                    b.Styles
                })
                .SingleOrDefaultAsync();

            if (blog == null)
            {
                return Content("");
            }

            var viewModel = new BlogStylesViewModel(blog.Styles);

            return View(viewModel);
        }
    }
}