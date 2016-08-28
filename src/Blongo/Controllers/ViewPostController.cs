namespace Blongo.Controllers
{
    using System;
    using System.Net.Mail;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using AkismetSdk;
    using AkismetSdk.Clients.CommentCheck;
    using Data;
    using MarkdownSharp;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.AspNetCore.Mvc;
    using ModelBinding;
    using Models.ViewPost;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using SendGrid;
    using Comment = Data.Comment;
    using Commenter = Models.ViewPost.Commenter;
    using Post = Data.Post;

    public class ViewPostController : Controller
    {
        private readonly CommentCheckClient _commentCheckClient;
        private readonly MongoClient _mongoClient;
        private readonly ISendGridShim _sendGrid;

        public ViewPostController(MongoClient mongoClient, CommentCheckClient commentCheckClient, ISendGridShim sendGrid)
        {
            _mongoClient = mongoClient;
            _commentCheckClient = commentCheckClient;
            _sendGrid = sendGrid;
        }

        [HttpGet]
        [Route("post/{id:objectId}", Name = "ViewPost")]
        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId id)
        {
            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var postsCollection = database.GetCollection<Post>(CollectionNames.Posts);
            var post = await postsCollection.Find(Builders<Post>.Filter.Where(p => p.Id == id))
                .Sort(Builders<Post>.Sort.Descending(c => c.PublishedAt))
                .Project(
                    p =>
                        new Models.ViewPost.Post(p.Id, p.Title, p.Description, p.Body, p.Tags.ToTagViewModels(),
                            p.CommentCount, p.PublishedAt, p.UrlSlug, p.Id.CreationTime, p.IsPublished))
                .SingleOrDefaultAsync();

            if (post == null)
            {
                return NotFound();
            }

            if (!User.Identity.IsAuthenticated && (!post.IsPublished || post.PublishedAt > DateTime.UtcNow))
            {
                return Unauthorized();
            }

            var createCommentModel = new CreateCommentModel();
            var commentsCollection = database.GetCollection<Comment>(CollectionNames.Comments);
            var comments =
                await commentsCollection.Find(Builders<Comment>.Filter.Where(c => c.PostId == id && !c.IsSpam))
                    .Sort(Builders<Comment>.Sort.Descending(c => c.Id))
                    .Project(
                        c =>
                            new Models.ViewPost.Comment(c.Id, c.PostId, c.Body,
                                new Commenter(c.Commenter.Name, c.Commenter.EmailAddress, c.Commenter.WebsiteUrl),
                                c.Id.CreationTime))
                    .ToListAsync();
            var previousPost =
                await postsCollection.Find(p => p.PublishedAt <= DateTime.UtcNow && p.PublishedAt < post.PublishedAt)
                    .Sort(Builders<Post>.Sort.Descending(p => p.PublishedAt))
                    .Project(p => new SiblingPost(p.Id, p.Title, p.UrlSlug))
                    .FirstOrDefaultAsync();
            var nextPost =
                await postsCollection.Find(p => p.PublishedAt <= DateTime.UtcNow && p.PublishedAt > post.PublishedAt)
                    .Sort(Builders<Post>.Sort.Descending(p => p.PublishedAt))
                    .Project(p => new SiblingPost(p.Id, p.Title, p.UrlSlug))
                    .FirstOrDefaultAsync();

            var viewModel = new ViewPostViewModel(post, comments, createCommentModel, previousPost, nextPost);

            return View("Index", viewModel);
        }

        [HttpPost]
        [Route("post/{id}/comment", Name = "CreateComment")]
        public async Task<IActionResult> Comment([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId id,
            CreateCommentModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return await Index(id);
            }

            var ipAddress = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
            var userAgent = Request.Headers["User-Agent"];

            var comment = new Comment
            {
                Body = new HtmlSanitizer(model.Body).SanitizedHtml,
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

            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var blogsCollection = database.GetCollection<Blog>(CollectionNames.Blogs);
            var postsCollection = database.GetCollection<Post>(CollectionNames.Posts);
            var commentsCollection = database.GetCollection<Comment>(CollectionNames.Comments);

            await commentsCollection.InsertOneAsync(comment);

            await
                postsCollection.UpdateOneAsync(Builders<Post>.Filter.Where(p => p.Id == id),
                    Builders<Post>.Update.Inc(p => p.CommentCount, 1));

            var blogUri = new Uri(Url.RouteUrl("ListPosts", null, Request.Scheme));
            await CommentCheck(comment.Id, blogUri, ipAddress, userAgent, model);

            var author = await blogsCollection.Find(Builders<Blog>.Filter.Empty)
                .Project(b => new
                {
                    b.Author.EmailAddress
                })
                .SingleAsync();
            var post = await postsCollection.Find(Builders<Post>.Filter.Where(p => p.Id == id))
                .Project(p => new
                {
                    p.Title
                })
                .SingleAsync();

            var emailBody = new StringBuilder();
            emailBody.Append($"Name: <strong>{model.Name}</strong><br />");
            emailBody.Append($"Email address: <strong>{model.EmailAddress}</strong><br />");

            if (!string.IsNullOrWhiteSpace(model.WebsiteUrl))
            {
                emailBody.Append($"Website URL: <strong>{model.WebsiteUrl}</strong><br />");
            }

            emailBody.Append("<br />");
            emailBody.Append(new Markdown().Transform(model.Body));

            try
            {
                await
                    _sendGrid.SendEmailAsync(new MailAddress(author.EmailAddress), $"New comment for \"{post.Title}\"", emailBody.ToString());
            }
            catch
            {
                // ignored
            }

            return Redirect(Url.RouteUrl("ViewPost", new {id}, null, null, $"comment-{comment.Id}"));
        }

        private async Task CommentCheck(ObjectId commentId, Uri blogUri, string ipAddress, string userAgent,
            CreateCommentModel model)
        {
            var akismetComment = new AkismetSdk.Comment(blogUri, ipAddress, userAgent)
            {
                CommentType = CommentType.Comment,
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

            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var commentsCollection = database.GetCollection<Comment>(CollectionNames.Comments);
            await
                commentsCollection.UpdateOneAsync(Builders<Comment>.Filter.Where(p => p.Id == commentId),
                    Builders<Comment>.Update.Set(c => c.IsAkismetSpam, result.IsSpam)
                        .Set(c => c.IsSpam, result.IsSpam));
        }
    }
}