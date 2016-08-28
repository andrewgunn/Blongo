namespace Blongo.ViewComponents
{
    using System;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Models.FuturePosts;
    using MongoDB.Driver;
    using Post = Data.Post;

    public class FuturePosts : ViewComponent
    {
        private readonly MongoClient _mongoClient;

        public FuturePosts(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<Post>(CollectionNames.Posts);
            var posts = await collection.Find(Builders<Post>
                .Filter.Where(p => p.IsPublished && p.PublishedAt >= DateTime.UtcNow.Date.AddDays(1)))
                .Sort(Builders<Post>.Sort.Ascending(p => p.PublishedAt))
                .Limit(5)
                .Project(p => new Models.FuturePosts.Post(p.Id, p.Title, p.PublishedAt))
                .ToListAsync();

            var viewModel = new FuturePostsViewModel(posts);

            return View(viewModel);
        }
    }
}