﻿namespace Blongo.Areas.Admin.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using ModelBinding;
    using Models.EditPost;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Tag = Data.Tag;

    [Area("admin")]
    [Authorize]
    [Route("admin/posts/edit/{id:objectId}", Name = "AdminEditPost")]
    public class EditPostController : Controller
    {
        private readonly MongoClient _mongoClient;

        public EditPostController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId id,
            string returnUrl = null)
        {
            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<Post>(CollectionNames.Posts);
            var model = await collection.Find(Builders<Post>.Filter.Where(p => p.Id == id))
                .Project(p => new EditPostModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Body = p.Body,
                    Scripts = p.Scripts,
                    Styles = p.Styles,
                    Tags = p.Tags.Select(t => t.Value).ToArray(),
                    PublishedAt = p.PublishedAt,
                    IsPublished = p.IsPublished
                })
                .SingleOrDefaultAsync();

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId id,
            EditPostModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                model.Id = id;

                return View(model);
            }

            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<Post>(CollectionNames.Posts);
            var update = Builders<Post>.Update
                .Set(p => p.Title, model.Title)
                .Set(p => p.Description, model.Description)
                .Set(p => p.Body, model.Body)
                .Set(p => p.Scripts, model.Scripts)
                .Set(p => p.Styles, model.Styles)
                .Set(p => p.Tags, model.Tags.Where(t => !string.IsNullOrWhiteSpace(t))
                    .Distinct()
                    .Select(t => new Tag
                    {
                        Value = t,
                        UrlSlug = new UrlSlug(t).Value
                    })
                    .ToList())
                .Set(p => p.PublishedAt,
                    new DateTime(model.PublishedAt.Value.Year, model.PublishedAt.Value.Month,
                        model.PublishedAt.Value.Day, model.PublishedAt.Value.Hour, model.PublishedAt.Value.Minute,
                        model.PublishedAt.Value.Second, DateTimeKind.Utc))
                .Set(p => p.IsPublished, model.IsPublished)
                .Set(p => p.LastUpdatedAt, DateTime.UtcNow)
                .Set(p => p.UrlSlug, new UrlSlug(model.Title).Value);
            await collection.UpdateOneAsync(Builders<Post>.Filter.Where(p => p.Id == id), update);

            return RedirectToLocal(returnUrl, id);
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