using Blongo.Models.BlogScripts;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Threading.Tasks;

namespace Blongo.ViewComponents
{
    public class BlogScripts : ViewComponent
    {
        public BlogScripts(MongoClient mongoClient)
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
                    b.Scripts
                })
                .SingleOrDefaultAsync();

            if (blog == null)
            {
                return Content("");
            }

            var viewModel = new BlogScriptsViewModel(blog.Scripts);

            return View(viewModel);
        }

        private readonly MongoClient _mongoClient;
    }
}
