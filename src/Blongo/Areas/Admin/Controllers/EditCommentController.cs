using Blongo.Areas.Admin.Models.EditComment;
using Blongo.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Blongo.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    [Route("admin/comments/edit/{id:objectId}", Name = "AdminEditComment")]
    public class EditCommentController : Controller
    {
        public EditCommentController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId id, string returnUrl = null)
        {
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var commentsCollection = database.GetCollection<Data.Comment>(Data.CollectionNames.Comments);
            var model = await commentsCollection.Find(Builders<Data.Comment>.Filter.Where(c => c.Id == id))
                .Project(c => new EditCommentModel
                {
                    Id =c.Id,
                    Name=c.Commenter.Name,
                        EmailAddress = c.Commenter.EmailAddress,
                        WebsiteUrl = c.Commenter.WebsiteUrl,
                    Body = c.Body,
                    PostId= c.PostId
                })
                .SingleOrDefaultAsync();

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId id, EditCommentModel model, string returnUrl = null)
        {
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var commentsCollection = database.GetCollection<Data.Comment>(Data.CollectionNames.Comments);

            if (!ModelState.IsValid)
            {
                model.Id = id;

                var comment = await commentsCollection.Find(Builders<Data.Comment>.Filter.Where(c => c.Id == id))
                    .Project(c => new
                    {
                        PostId = c.PostId
                    })
                    .SingleOrDefaultAsync();

                if (comment == null)
                {
                    return NotFound();
                }

                model.PostId = comment.PostId;

                return View(model);
            }

            var update = Builders<Data.Comment>.Update
                .Set(c => c.Commenter.Name, model.Name)
                .Set(c => c.Commenter.EmailAddress, model.EmailAddress)
                .Set(c => c.Commenter.WebsiteUrl, model.WebsiteUrl)
                .Set(c => c.Body, model.Body);
            await commentsCollection.UpdateOneAsync(Builders<Data.Comment>.Filter.Where(c => c.Id == id), update);

            return RedirectToLocal(returnUrl);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToRoute("AdminListComments", new { id = "" });
            }
        }

        private readonly MongoClient _mongoClient;
    }
}
