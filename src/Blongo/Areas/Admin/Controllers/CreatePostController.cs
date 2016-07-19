using Blongo.Areas.Admin.Models.CreatePost;
using Blongo.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Blongo.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    [Route("admin/posts/create", Name = "AdminCreatePost")]
    [ServiceFilter(typeof(UserDataFilter))]
    public class CreatePostController : Controller
    {
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
        public async Task<IActionResult> Index(CreatePostModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var post = new Data.Post
            {
                Title = model.Title,
                Description = model.Description,
                Tags = model.Tags.Where(t => !string.IsNullOrWhiteSpace(t))
                    .Distinct()
                    .Select(t => new Data.Tag
                    {
                        Value = t,
                        UrlSlug = new UrlSlug(t).Value
                    })
                    .ToList(),
                Body = model.Body,
                Scripts = model.Scripts,
                Styles = model.Styles,
                PublishedAt = model.PublishedAt.Value,
                UrlSlug = new UrlSlug(model.Title).Value,
                IsPublished = model.IsPublished
            };
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.Post>(Data.CollectionNames.Posts);
            await collection.InsertOneAsync(post);

            return RedirectToRoute("AdminListPosts", new { post.Id });
        }

        private readonly MongoClient _mongoClient;
    }
}
