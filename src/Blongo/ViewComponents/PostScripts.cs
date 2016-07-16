using Blongo.Models.PostScripts;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Threading.Tasks;

namespace Blongo.ViewComponents
{
    public class PostScripts : ViewComponent
    {
        public PostScripts(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public async Task<IViewComponentResult> InvokeAsync(ObjectId postId)
        {
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.Post>(Data.CollectionNames.Posts);
            var post = await collection.Find(Builders<Data.Post>.Filter.Where(p => p.Id == postId))
                .Project(p => new
                {
                    Scripts = p.Scripts
                })
                .SingleOrDefaultAsync();

            if (post == null)
            {
                return Content("");
            }

            var viewModel = new PostScriptsViewModel(post.Scripts);

            return View(viewModel);
        }

        private readonly MongoClient _mongoClient;
    }
}
