using AkismetSdk.Clients.CommentCheck;
using Blongo.Filters;
using Blongo.ModelBinding;
using Blongo.Models.ViewPost;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Blongo.Controllers
{
    [ServiceFilter(typeof(BlogDataFilter))]
    [ServiceFilter(typeof(UserDataFilter))]
    public class ViewPostController : Controller
    {
        public ViewPostController(MongoClient mongoClient, CommentCheckClient commentCheckClient)
        {
            _mongoClient = mongoClient;
            _commentCheckClient = commentCheckClient;
        }

        [HttpGet]
        [Route("post/{id:objectId}", Name = "ViewPost")]
        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId id)
        {
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var postsCollection = database.GetCollection<Data.Post>(Data.CollectionNames.Posts);
            var filter = Builders<Data.Post>.Filter.Where(p => p.Id == id && (User.Identity.IsAuthenticated || (p.IsPublished && p.PublishedAt <= DateTime.UtcNow)));

            var post = await postsCollection.Find(filter)
                .Project(p => new Post(p.Id, p.Title, p.Description, p.Body, p.Tags.ToTagViewModels(), p.CommentCount, p.PublishedAt, p.UrlSlug, p.CreatedAt, p.IsPublished))
                .SingleOrDefaultAsync();

            if (post == null)
            {
                return NotFound();
            }

            var createCommentModel = new CreateCommentModel();
            var commentsCollection = database.GetCollection<Data.Comment>(Data.CollectionNames.Comments);
            var comments = await commentsCollection.Find(Builders<Data.Comment>.Filter.Where(c => c.PostId == id && !c.IsSpam))
                .Sort(Builders<Data.Comment>.Sort.Descending(c => c.CreatedAt))
                .Project(c => new Comment(c.Id, c.PostId, c.Body, new Commenter(c.Commenter.Name, c.Commenter.EmailAddress, c.Commenter.WebsiteUrl), c.CreatedAt))
                .ToListAsync();
            var previousPost = await postsCollection.Find(p => p.PublishedAt <= DateTime.UtcNow && p.PublishedAt < post.PublishedAt)
                .Sort(Builders<Data.Post>.Sort.Descending(p => p.PublishedAt))
                .Project(p => new SiblingPost(p.Id, p.Title, p.UrlSlug))
                .FirstOrDefaultAsync();
            var nextPost = await postsCollection.Find(p => p.PublishedAt <= DateTime.UtcNow && p.PublishedAt > post.PublishedAt)
                .Sort(Builders<Data.Post>.Sort.Descending(p => p.PublishedAt))
                .Project(p => new SiblingPost(p.Id, p.Title, p.UrlSlug))
                .FirstOrDefaultAsync();

            var viewModel = new ViewPostViewModel(post, comments, createCommentModel, previousPost, nextPost);

            return View("Index", viewModel);
        }

        [HttpPost]
        [Route("post/{id}/comment", Name = "CreateComment")]
        public async Task<IActionResult> Comment([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId id, CreateCommentModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return await Index(id);
            }

            var ipAddress = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
            var userAgent = Request.Headers["User-Agent"];

            var comment = new Data.Comment
            {
                Body = new CommentHtmlStripper(model.Body).StrippedHtml,
                Commenter = new Data.Commenter
                {
                    Name = model.Name,
                    EmailAddress = model.EmailAddress,
                    WebsiteUrl = model.WebsiteUrl,
                    IpAddress = ipAddress,
                    UserAgent = userAgent
                },
                PostId = id
            };

            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var blogsCollection = database.GetCollection<Data.Blog>(Data.CollectionNames.Blogs);
            var postsCollection = database.GetCollection<Data.Post>(Data.CollectionNames.Posts);
            var commentsCollection = database.GetCollection<Data.Comment>(Data.CollectionNames.Comments);

            await commentsCollection.InsertOneAsync(comment);

            await postsCollection.UpdateOneAsync(Builders<Data.Post>.Filter.Where(p => p.Id == id), Builders<Data.Post>.Update.Inc(p => p.CommentCount, 1));

            var blogUri = new Uri(Url.RouteUrl("ListPosts", null, Request.Scheme));
            await CommentCheck(comment.Id, blogUri, ipAddress, userAgent, model);

            return Redirect(Url.RouteUrl("ViewPost", new { id }, null, null, $"comment-{comment.Id}"));
        }

        private async Task CommentCheck(ObjectId commentId, Uri blogUri, string ipAddress, string userAgent, CreateCommentModel model)
        {
            var akismetComment = new AkismetSdk.Comment(blogUri, ipAddress, userAgent)
            {
                CommentType = AkismetSdk.CommentType.Comment,
                Name = model.Name,
                EmailAddress = model.EmailAddress,
                WebsiteUrl = model.WebsiteUrl,
                Body = model.Body
            };

            CommentCheckResult result;

            try
            {
                result = await _commentCheckClient.PostAsync(akismetComment, CancellationToken.None);
            }
            catch
            {
                return;
            }

            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var commentsCollection = database.GetCollection<Data.Comment>(Data.CollectionNames.Comments);
            await commentsCollection.UpdateOneAsync(Builders<Data.Comment>.Filter.Where(p => p.Id == commentId), Builders<Data.Comment>.Update.Set(c => c.IsAkismetSpam, result.IsSpam)
                .Set(c => c.IsSpam, result.IsSpam));
        }

        private readonly CommentCheckClient _commentCheckClient;
        private readonly MongoClient _mongoClient;
    }
}