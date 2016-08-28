namespace Blongo.ViewComponents
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Models.Tags;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;
    using Tag = Models.Tags.Tag;

    public class Tags : ViewComponent
    {
        private readonly MongoClient _mongoClient;

        public Tags(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<Post>(CollectionNames.Posts);
            var tags = await collection.AsQueryable()
                .Where(p => p.IsPublished && p.PublishedAt <= DateTime.UtcNow)
                .SelectMany(p => p.Tags)
                .GroupBy(t => new {t.Value, t.UrlSlug})
                .OrderByDescending(t => t.Count())
                .ThenBy(t => t.Key.Value)
                .Select(x => new Tag(x.Key.Value, x.Key.UrlSlug, x.Count()))
                .ToListAsync();

            var viewModel = new TagsViewModel(tags);

            return View(viewModel);
        }
    }
}