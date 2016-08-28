namespace Blongo.Areas.Admin.Controllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AkismetSdk;
    using AkismetSdk.Clients.SubmitSpam;
    using Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using ModelBinding;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Comment = Data.Comment;

    [Area("admin")]
    [Authorize]
    [Route("admin/comments/ham/{id:objectId}", Name = "AdminHamComment")]
    public class HamCommentController : Controller
    {
        private readonly MongoClient _mongoClient;
        private readonly SubmitSpamClient _submitSpamClient;

        public HamCommentController(MongoClient mongoClient, SubmitSpamClient submitSpamClient)
        {
            _mongoClient = mongoClient;
            _submitSpamClient = submitSpamClient;
        }

        [HttpPost]
        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId id,
            string returnUrl = null)
        {
            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);

            var commentsCollection = database.GetCollection<Comment>(CollectionNames.Comments);
            var comment = await commentsCollection.Find(Builders<Comment>.Filter.Where(c => c.Id == id))
                .SingleOrDefaultAsync();
            await
                commentsCollection.UpdateOneAsync(Builders<Comment>.Filter.Where(c => c.Id == id),
                    Builders<Comment>.Update.Set(p => p.IsSpam, false));

            var postsCollection = database.GetCollection<Post>(CollectionNames.Posts);
            await
                postsCollection.UpdateOneAsync(Builders<Post>.Filter.Where(p => p.Id == comment.PostId),
                    Builders<Post>.Update.Inc(p => p.CommentCount, 1));

            var blogUri = new Uri(Url.RouteUrl("ListPosts", null, Request.Scheme));
            var akismetComment = new AkismetSdk.Comment(blogUri, comment.Commenter.IpAddress,
                comment.Commenter.UserAgent)
            {
                CommentType = CommentType.Comment,
                Name = comment.Commenter.Name,
                EmailAddress = comment.Commenter.EmailAddress,
                WebsiteUrl = comment.Commenter.WebsiteUrl,
                Body = comment.Body
            };

            try
            {
                await _submitSpamClient.PostAsync(akismetComment, CancellationToken.None);
            }
            catch
            {
                // ignored
            }

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