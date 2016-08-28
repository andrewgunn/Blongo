namespace Blongo.Areas.Admin.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.CreatePost;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Tag = Data.Tag;

    [Area("admin")]
    [Authorize]
    [Route("admin/posts/create", Name = "AdminCreatePost")]
    public class CreatePostController : Controller
    {
        private readonly MongoClient _mongoClient;

        public CreatePostController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new CreatePostModel
            {
                PublishedAt = DateTime.UtcNow
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreatePostModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<Post>(CollectionNames.Posts);

            var post = new Post
            {
                Title = model.Title,
                Description = model.Description,
                Tags = model.Tags.Where(t => !string.IsNullOrWhiteSpace(t))
                    .Distinct()
                    .Select(t => new Tag
                    {
                        Value = t,
                        UrlSlug = new UrlSlug(t).Value
                    })
                    .ToList(),
                Body = model.Body,
                Scripts = model.Scripts,
                Styles = model.Styles,
                PublishedAt =
                    new DateTime(model.PublishedAt.Value.Year, model.PublishedAt.Value.Month,
                        model.PublishedAt.Value.Day, model.PublishedAt.Value.Hour, model.PublishedAt.Value.Minute,
                        model.PublishedAt.Value.Second, DateTimeKind.Utc),
                UrlSlug = new UrlSlug(model.Title).Value,
                IsPublished = model.IsPublished
            };

            await collection.InsertOneAsync(post);

            return RedirectToLocal(returnUrl, post.Id);
        }

        private IActionResult RedirectToLocal(string returnUrl, ObjectId postId)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToRoute("AdminListPosts", new {id = postId});
        }
    }
}