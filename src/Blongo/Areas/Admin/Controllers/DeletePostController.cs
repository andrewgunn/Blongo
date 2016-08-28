namespace Blongo.Areas.Admin.Controllers
{
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using ModelBinding;
    using MongoDB.Bson;
    using MongoDB.Driver;

    [Area("admin")]
    [Authorize]
    [Route("admin/posts/delete/{id:objectId}", Name = "AdminDeletePost")]
    public class DeletePostController : Controller
    {
        private readonly MongoClient _mongoClient;

        public DeletePostController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpPost]
        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId id,
            string returnUrl = null)
        {
            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var postsCollection = database.GetCollection<Post>(CollectionNames.Posts);
            await postsCollection.DeleteOneAsync(Builders<Post>.Filter.Where(p => p.Id == id));
            var commentsCollection = database.GetCollection<Comment>(CollectionNames.Comments);
            await commentsCollection.DeleteManyAsync(Builders<Comment>.Filter.Where(c => c.PostId == id));

            return RedirectToLocal(returnUrl);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToRoute("AdminListPosts", new {id = ""});
        }
    }
}