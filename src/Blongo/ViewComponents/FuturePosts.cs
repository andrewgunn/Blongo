using Blongo.Models.FuturePosts;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Blongo.ViewComponents
{
    public class FuturePosts : ViewComponent
    {
        public FuturePosts(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.Post>(Data.CollectionNames.Posts);
            var posts = await collection.Find(Builders<Data.Post>
                .Filter.Where(p => p.IsPublished && p.PublishedAt >= DateTime.UtcNow.Date.AddDays(1)))
                .Sort(Builders<Data.Post>.Sort.Ascending(p => p.PublishedAt))
                .Limit(5)
                .Project(p => new Post(p.Id, p.Title, p.PublishedAt))
                .ToListAsync();

            var viewModel = new FuturePostsViewModel(posts);

            return View(viewModel);
        }

        private readonly MongoClient _mongoClient;
    }
}
