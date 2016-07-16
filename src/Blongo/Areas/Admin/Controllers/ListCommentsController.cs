using Blongo.Areas.Admin.Models.ListComments;
using Blongo.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using MongoDB.Bson;
using Blongo.ModelBinding;
using System.Linq;

namespace Blongo.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    [Route("admin/comments/{id?}", Name = "AdminListComments")]
    [ServiceFilter(typeof(UserDataFilter))]
    public class ListCommentsController : Controller
    {
        public ListCommentsController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId? id, [ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId? postId, [FromQuery(Name = "p")] int pageNumber = 1)
        {
            if (pageNumber < 1)
            {
                return NotFound();
            }

            const int pageSize = 10;
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.Comment>(Data.CollectionNames.Comments);
            var filter = Builders<Data.Comment>.Filter.Empty;

            if (postId != null)
            {
                filter = Builders<Data.Comment>.Filter.Where(c => c.PostId == postId);
            }

            var totalCount = await collection.CountAsync(filter);
            var maximumPageNumber = Math.Max(1, (int)Math.Ceiling((double)totalCount / pageSize));

            if (pageNumber > maximumPageNumber)
            {
                return NotFound();
            }

            var comments = new List<Comment>();
            SelectedComment selectedComment = null;

            if (totalCount > 0)
            {
                comments = await collection.Find(filter)
                    .Sort(Builders<Data.Comment>.Sort.Descending(c => c.CreatedAt))
                    .Skip((pageNumber - 1) * pageSize)
                    .Limit(pageSize)
                    .Project(c => new Comment(c.Id, c.PostId, c.Body, new Commenter(c.Commenter.Name, c.Commenter.EmailAddress, c.Commenter.WebsiteUrl), c.IsAkismetSpam, c.IsSpam, c.CreatedAt))
                    .ToListAsync();

                var commentId = comments.Any(p => p.Id == id) ? id : comments.First().Id;

                selectedComment = await collection.Find(Builders<Data.Comment>.Filter.Where(p => p.Id == commentId))
                    .Project(c => new SelectedComment(c.Id, c.PostId, c.Body, c.IsSpam, c.Commenter.WebsiteUrl))
                    .SingleOrDefaultAsync();
            }

            var paging = new Paging(pageNumber, pageSize, maximumPageNumber);
            var viewModel = new ListCommentsViewModel(comments, paging, selectedComment);

            return View(viewModel);
        }

        private readonly MongoClient _mongoClient;
    }
}
