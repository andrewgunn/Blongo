using Blongo.Models.ListPosts;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Blongo.Filters;
using System.Linq;

namespace Blongo.Controllers
{
    [Route("", Name = "ListPosts")]
    [Route("tag/{slug}", Name = "ListPostsByTag")]
    [ServiceFilter(typeof(BlogDataFilter))]
    [ServiceFilter(typeof(UserDataFilter))]
    public class ListPostsController : Controller
    {
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
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.Post>(Data.CollectionNames.Posts);
            var filter = Builders<Data.Post>.Filter.Where(p => p.IsPublished && p.PublishedAt <= DateTime.UtcNow);

            if (!string.IsNullOrWhiteSpace(slug))
            {
                filter = Builders<Data.Post>.Filter.And(filter, Builders<Data.Post>.Filter.Where(p => p.Tags.Any(t => t.UrlSlug == slug)));
            }

            var totalCount = await collection.CountAsync(filter);
            var maximumPageNumber = Math.Max(1, (int)Math.Ceiling((double)totalCount / pageSize));

            if (pageNumber > maximumPageNumber)
            {
                return NotFound();
            }

            var posts = new List<Post>();

            if (totalCount > 0)
            {
                posts = await collection.Find(filter)
                    .Sort(Builders<Data.Post>.Sort.Descending(p => p.PublishedAt))
                    .Skip((pageNumber - 1) * pageSize)
                    .Limit(pageSize)
                    .Project(p => new Post(p.Id, p.Title, p.Body, p.Tags.ToTagViewModels(), p.CommentCount, p.UrlSlug, p.Id.CreationTime.ToUniversalTime(), p.PublishedAt))
                    .ToListAsync();
            }

            var paging = new Paging(pageNumber > 1 ? pageNumber - 1 : (int?)null, pageNumber < maximumPageNumber ? pageNumber + 1 : (int?)null);
            var viewModel = new ListPostsViewModel(posts, paging);

            return View(viewModel);
        }

        private readonly MongoClient _mongoClient;
    }
}
