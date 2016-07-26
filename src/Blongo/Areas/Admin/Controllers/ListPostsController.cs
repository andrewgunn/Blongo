using Blongo.Areas.Admin.Models.ListPosts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Blongo.ModelBinding;

namespace Blongo.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    [Route("admin/{id:objectid?}", Name = "AdminListPosts")]
    public class ListPostsController : Controller
    {
        public ListPostsController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId? id, [FromQuery(Name = "p")] int pageNumber = 1)
        {
            if (pageNumber < 1)
            {
                return NotFound();
            }

            const int pageSize = 10;
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.Post>(Data.CollectionNames.Posts);
            var filter = Builders<Data.Post>.Filter.Empty;
            var totalCount = await collection.CountAsync(filter);
            var maximumPageNumber = Math.Max(1, (int)Math.Ceiling((double)totalCount / pageSize));

            if (pageNumber > maximumPageNumber)
            {
                return NotFound();
            }

            var posts = new List<Post>();
            SelectedPost selectedPost = null;

            if (totalCount > 0)
            {
                posts = await collection.Find(filter)
                    .Sort(Builders<Data.Post>.Sort.Descending(p => p.Id))
                    .Skip((pageNumber - 1) * pageSize)
                    .Limit(pageSize)
                    .Project(p => new Post(p.Id, p.Title, p.PublishedAt, p.IsPublished))
                    .ToListAsync();

                var postId = posts.Any(p => p.Id == id) ?  id : posts.First().Id;

                selectedPost = await collection.Find(Builders<Data.Post>.Filter.Where(p => p.Id == postId))
                    .Project(p => new SelectedPost(p.Id, p.Body, p.IsPublished))
                    .SingleOrDefaultAsync();
            }

            var paging = new Paging(pageNumber, pageSize, maximumPageNumber);
            var viewModel = new ListPostsViewModel(posts, paging, selectedPost);

            return View(viewModel);
        }

        private readonly MongoClient _mongoClient;
    }
}
