using AkismetSdk.Clients.SubmitHam;
using Blongo.Filters;
using Blongo.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Blongo.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    [Route("admin/comments/spam/{id:objectId}", Name = "AdminSpamComment")]
    [ServiceFilter(typeof(UserDataFilter))]
    public class SpamCommentController : Controller
    {
        public SpamCommentController(MongoClient mongoClient, SubmitHamClient submitHamClient)
        {
            _mongoClient = mongoClient;
            _submitHamClient = submitHamClient;
        }

        [HttpPost]
        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId id, string returnUrl = null)
        {
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var blogsCollection = database.GetCollection<Data.Blog>(Data.CollectionNames.Blogs);
            var postsCollection = database.GetCollection<Data.Post>(Data.CollectionNames.Posts);
            var commentsCollection = database.GetCollection<Data.Comment>(Data.CollectionNames.Comments);

            var comment = await commentsCollection.Find(Builders<Data.Comment>.Filter.Where(c => c.Id == id))
                .SingleOrDefaultAsync();

            await commentsCollection.UpdateOneAsync(Builders<Data.Comment>.Filter.Where(c => c.Id == id), Builders<Data.Comment>.Update.Set(p => p.IsSpam, true));

            await postsCollection.UpdateOneAsync(Builders<Data.Post>.Filter.Where(p => p.Id == comment.PostId), Builders<Data.Post>.Update.Inc(p => p.CommentCount, -1));

            var blogUri = new Uri(Url.RouteUrl("ListPosts", null, Request.Scheme));
            var akismetComment = new AkismetSdk.Comment(blogUri, comment.Commenter.IpAddress, comment.Commenter.UserAgent)
            {
                CommentType = AkismetSdk.CommentType.Comment,
                Name = comment.Commenter.Name,
                EmailAddress = comment.Commenter.EmailAddress,
                WebsiteUrl = comment.Commenter.WebsiteUrl,
                Body = comment.Body
            };

            try
            {
                await _submitHamClient.PostAsync(akismetComment, CancellationToken.None);
            }
            catch
            {
            }

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
        private SubmitHamClient _submitHamClient;
    }
}