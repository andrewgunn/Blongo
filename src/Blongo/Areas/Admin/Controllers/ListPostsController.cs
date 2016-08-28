namespace Blongo.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using ModelBinding;
    using Models.ListPosts;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Post = Data.Post;

    [Area("admin")]
    [Authorize]
    [Route("admin/{id:objectid?}", Name = "AdminListPosts")]
    public class ListPostsController : Controller
    {
        private readonly MongoClient _mongoClient;

        public ListPostsController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId? id,
            [FromQuery(Name = "p")] int pageNumber = 1)
        {
            if (pageNumber < 1)
            {
                return NotFound();
            }

            const int pageSize = 10;
            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<Post>(CollectionNames.Posts);
            var filter = Builders<Post>.Filter.Empty;
            var totalCount = await collection.CountAsync(filter);
            var maximumPageNumber = Math.Max(1, (int) Math.Ceiling((double) totalCount/pageSize));

            if (pageNumber > maximumPageNumber)
            {
                return NotFound();
            }

            var posts = new List<Models.ListPosts.Post>();
            SelectedPost selectedPost = null;

            if (totalCount > 0)
            {
                posts = await collection.Find(filter)
                    .Sort(Builders<Post>.Sort.Descending(p => p.Id))
                    .Skip((pageNumber - 1)*pageSize)
                    .Limit(pageSize)
                    .Project(p => new Models.ListPosts.Post(p.Id, p.Title, p.PublishedAt, p.IsPublished))
                    .ToListAsync();

                var postId = posts.Any(p => p.Id == id) ? id : posts.First().Id;

                selectedPost = await collection.Find(Builders<Post>.Filter.Where(p => p.Id == postId))
                    .Project(p => new SelectedPost(p.Id, p.Body, p.IsPublished))
                    .SingleOrDefaultAsync();
            }

            var paging = new Paging(pageNumber, pageSize, maximumPageNumber);
            var viewModel = new ListPostsViewModel(posts, paging, selectedPost);

            return View(viewModel);
        }
    }
}