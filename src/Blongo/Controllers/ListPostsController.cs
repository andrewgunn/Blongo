namespace Blongo.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Models.ListPosts;
    using MongoDB.Driver;
    using Post = Data.Post;

    [Route("", Name = "ListPosts")]
    [Route("tag/{slug}", Name = "ListPostsByTag")]
    public class ListPostsController : Controller
    {
        private readonly MongoClient _mongoClient;

        public ListPostsController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string slug, [FromQuery(Name = "p")] int pageNumber = 1)
        {
            if (pageNumber < 1)
            {
                return NotFound();
            }

            const int pageSize = 10;
            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<Post>(CollectionNames.Posts);
            var filter = Builders<Post>.Filter.Where(p => p.IsPublished && p.PublishedAt <= DateTime.UtcNow);

            if (!string.IsNullOrWhiteSpace(slug))
            {
                filter = Builders<Post>.Filter.And(filter,
                    Builders<Post>.Filter.Where(p => p.Tags.Any(t => t.UrlSlug == slug)));
            }

            var totalCount = await collection.CountAsync(filter);
            var maximumPageNumber = Math.Max(1, (int) Math.Ceiling((double) totalCount/pageSize));

            if (pageNumber > maximumPageNumber)
            {
                return NotFound();
            }

            var posts = new List<Models.ListPosts.Post>();

            if (totalCount > 0)
            {
                posts = await collection.Find(filter)
                    .Sort(Builders<Post>.Sort.Descending(p => p.PublishedAt))
                    .Skip((pageNumber - 1)*pageSize)
                    .Limit(pageSize)
                    .Project(
                        p =>
                            new Models.ListPosts.Post(p.Id, p.Title, p.Body, p.Tags.ToTagViewModels(), p.CommentCount,
                                p.UrlSlug, p.Id.CreationTime, p.PublishedAt))
                    .ToListAsync();
            }

            var paging = new Paging(pageNumber > 1 ? pageNumber - 1 : (int?) null,
                pageNumber < maximumPageNumber ? pageNumber + 1 : (int?) null);
            var viewModel = new ListPostsViewModel(posts, paging);

            return View(viewModel);
        }
    }
}