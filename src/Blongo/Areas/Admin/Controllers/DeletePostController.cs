using Blongo.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Blongo.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    [Route("admin/posts/delete/{id:objectId}", Name = "AdminDeletePost")]
    public class DeletePostController : Controller
    {
        public DeletePostController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpPost]
        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId id, string returnUrl = null)
        {
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);

            var postsCollection = database.GetCollection<Data.Post>(Data.CollectionNames.Posts);
            await postsCollection.DeleteOneAsync(Builders<Data.Post>.Filter.Where(p => p.Id == id));

            var commentsCollection = database.GetCollection<Data.Comment>(Data.CollectionNames.Comments);
            await commentsCollection.DeleteManyAsync(Builders<Data.Comment>.Filter.Where(c => c.PostId == id));

            return RedirectToLocal(returnUrl);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToRoute("AdminListPosts", new { id = "" });
            }
        }

        private readonly MongoClient _mongoClient;
    }
}
