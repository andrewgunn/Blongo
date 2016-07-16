using Blongo.Models.Tags;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Blongo.ViewComponents
{
    public class Tags : ViewComponent
    {
        public Tags(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.Post>(Data.CollectionNames.Posts);
            var tags = await collection.AsQueryable()
                .Where(p => p.IsPublished && p.PublishedAt <= DateTime.UtcNow)
                .SelectMany(p => p.Tags)
                .GroupBy(t => new { t.Value, t.UrlSlug })
                .OrderByDescending(t => t.Count())
                .ThenBy(t => t.Key.Value)
                .Select(x => new Models.Tags.Tag(x.Key.Value, x.Key.UrlSlug, x.Count()))
                .ToListAsync();

            var viewModel = new TagsViewModel(tags);

            return View(viewModel);
        }

        private readonly MongoClient _mongoClient;
    }
}
