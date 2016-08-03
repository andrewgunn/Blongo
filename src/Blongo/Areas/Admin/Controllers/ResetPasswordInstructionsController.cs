using Blongo.Areas.Admin.Models.ResetPasswordInstructions;
using Blongo.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Blongo.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/forgotpassword/{id:objectId}", Name = "AdminResetPasswordInstructions")]
    public class ResetPasswordInstructionsController : Controller
    {
        public ResetPasswordInstructionsController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId id)
        {
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.ResetPasswordLink>(Data.CollectionNames.ResetPasswordLinks);

            var resetPasswordLink = await collection.Find(Builders<Data.ResetPasswordLink>.Filter.Where(rpl => rpl.Id == id))
                .SingleOrDefaultAsync();

            if (resetPasswordLink == null || resetPasswordLink.ExpiresAt < DateTime.UtcNow)
            {
                return NotFound();
            }

            var model = new ResetPasswordInstructionsViewModel(resetPasswordLink.ExpiresAt);

            return View(model);
        }

        private readonly MongoClient _mongoClient;
    }
}
