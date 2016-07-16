using Blongo.Filters;
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
    [Route("admin/comments/delete/{id:objectId}", Name = "AdminDeleteComment")]
    [ServiceFilter(typeof(UserDataFilter))]
    public class DeleteCommentController : Controller
    {
        public DeleteCommentController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpPost]
        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId id, string returnUrl = null)
        {
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);

            var commentsCollection = database.GetCollection<Data.Comment>(Data.CollectionNames.Comments);

            var comment = await commentsCollection.Find(Builders<Data.Comment>.Filter.Where(c => c.Id == id))
                .Project(c => new
                {
                    PostId = c.PostId
                })
                .SingleOrDefaultAsync();

            await commentsCollection.DeleteOneAsync(Builders<Data.Comment>.Filter.Where(c => c.Id == id));

            var postsCollection = database.GetCollection<Data.Post>(Data.CollectionNames.Posts);
            await postsCollection.UpdateOneAsync(Builders<Data.Post>.Filter.Where(p => p.Id == comment.PostId), Builders<Data.Post>.Update.Inc(p => p.CommentCount, -1));

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
                return RedirectToRoute("AdminListComments", new { id = "" });
            }
        }

        private readonly MongoClient _mongoClient;
    }
}