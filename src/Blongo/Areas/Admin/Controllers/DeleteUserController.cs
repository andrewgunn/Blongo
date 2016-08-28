namespace Blongo.Areas.Admin.Controllers
{
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using ModelBinding;
    using MongoDB.Bson;
    using MongoDB.Driver;

    [Area("admin")]
    [Authorize(Roles = UserRoles.Administrator)]
    [Route("admin/users/delete/{id:objectId}", Name = "AdminDeleteUser")]
    public class DeleteUserController : Controller
    {
        private readonly MongoClient _mongoClient;

        public DeleteUserController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpPost]
        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId id,
            string returnUrl = null)
        {
            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<User>(CollectionNames.Users);

            if (await collection.CountAsync(Builders<User>.Filter.Empty) == 1)
            {
                return RedirectToLocal(returnUrl);
            }

            await
                collection.DeleteOneAsync(
                    Builders<User>.Filter.Where(u => u.Id == id && u.EmailAddress != User.Identity.Name));

            return RedirectToLocal(returnUrl);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToRoute("AdminListUsers", new {id = ""});
        }
    }
}