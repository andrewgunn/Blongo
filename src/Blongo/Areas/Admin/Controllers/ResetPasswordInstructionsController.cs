namespace Blongo.Areas.Admin.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using ModelBinding;
    using Models.ResetPasswordInstructions;
    using MongoDB.Bson;
    using MongoDB.Driver;

    [Area("admin")]
    [Route("admin/forgotpassword/{id:objectId}", Name = "AdminResetPasswordInstructions")]
    public class ResetPasswordInstructionsController : Controller
    {
        private readonly MongoClient _mongoClient;

        public ResetPasswordInstructionsController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId id)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToRoute("AdminListPosts");
            }

            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<ResetPasswordLink>(CollectionNames.ResetPasswordLinks);

            var resetPasswordLink = await collection.Find(Builders<ResetPasswordLink>.Filter.Where(rpl => rpl.Id == id))
                .SingleOrDefaultAsync();

            if (resetPasswordLink == null || resetPasswordLink.ExpiresAt < DateTime.UtcNow)
            {
                return NotFound();
            }

            var model = new ResetPasswordInstructionsViewModel(resetPasswordLink.ExpiresAt);

            return View(model);
        }
    }
}