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
    using Models.ListComments;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Comment = Data.Comment;
    using Commenter = Models.ListComments.Commenter;

    [Area("admin")]
    [Authorize]
    [Route("admin/comments/{id?}", Name = "AdminListComments")]
    public class ListCommentsController : Controller
    {
        private readonly MongoClient _mongoClient;

        public ListCommentsController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId? id,
            [ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId? postId,
            [FromQuery(Name = "p")] int pageNumber = 1)
        {
            if (pageNumber < 1)
            {
                return NotFound();
            }

            const int pageSize = 10;
            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<Comment>(CollectionNames.Comments);
            var filter = Builders<Comment>.Filter.Empty;

            if (postId != null)
            {
                filter = Builders<Comment>.Filter.Where(c => c.PostId == postId);
            }

            var totalCount = await collection.CountAsync(filter);
            var maximumPageNumber = Math.Max(1, (int) Math.Ceiling((double) totalCount/pageSize));

            if (pageNumber > maximumPageNumber)
            {
                return NotFound();
            }

            var comments = new List<Models.ListComments.Comment>();
            SelectedComment selectedComment = null;

            if (totalCount > 0)
            {
                comments = await collection.Find(filter)
                    .Sort(Builders<Comment>.Sort.Descending(c => c.Id))
                    .Skip((pageNumber - 1)*pageSize)
                    .Limit(pageSize)
                    .Project(
                        c =>
                            new Models.ListComments.Comment(c.Id, c.PostId, c.Body,
                                new Commenter(c.Commenter.Name, c.Commenter.EmailAddress, c.Commenter.WebsiteUrl),
                                c.IsAkismetSpam, c.IsSpam, c.Id.CreationTime))
                    .ToListAsync();

                var commentId = comments.Any(p => p.Id == id) ? id : comments.First().Id;

                selectedComment = await collection.Find(Builders<Comment>.Filter.Where(p => p.Id == commentId))
                    .Project(c => new SelectedComment(c.Id, c.PostId, c.Body, c.IsSpam, c.Commenter.WebsiteUrl))
                    .SingleOrDefaultAsync();
            }

            var paging = new Paging(pageNumber, pageSize, maximumPageNumber);
            var viewModel = new ListCommentsViewModel(comments, paging, selectedComment);

            return View(viewModel);
        }
    }
}