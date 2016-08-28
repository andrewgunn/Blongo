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
    [Route("admin/comments/delete/{id:objectId}", Name = "AdminDeleteComment")]
    public class DeleteCommentController : Controller
    {
        private readonly MongoClient _mongoClient;

        public DeleteCommentController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpPost]
        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId id,
            string returnUrl = null)
        {
            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);

            var commentsCollection = database.GetCollection<Comment>(CollectionNames.Comments);
            var comment = await commentsCollection.Find(Builders<Comment>.Filter.Where(c => c.Id == id))
                .Project(c => new
                {
                    c.PostId
                })
                .SingleOrDefaultAsync();
            await commentsCollection.DeleteOneAsync(Builders<Comment>.Filter.Where(c => c.Id == id));

            var postsCollection = database.GetCollection<Post>(CollectionNames.Posts);
            await
                postsCollection.UpdateOneAsync(Builders<Post>.Filter.Where(p => p.Id == comment.PostId),
                    Builders<Post>.Update.Inc(p => p.CommentCount, -1));

            return RedirectToLocal(returnUrl);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToRoute("AdminListComments", new {id = ""});
        }
    }
}