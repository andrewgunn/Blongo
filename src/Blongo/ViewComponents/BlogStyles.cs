using Blongo.Models.BlogStyles;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Threading.Tasks;

namespace Blongo.ViewComponents
{
    public class BlogStyles : ViewComponent
    {
        public BlogStyles(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.Blog>(Data.CollectionNames.Blogs);
            var blog = await collection.Find(Builders<Data.Blog>.Filter.Empty)
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

        private readonly MongoClient _mongoClient;
    }
}
