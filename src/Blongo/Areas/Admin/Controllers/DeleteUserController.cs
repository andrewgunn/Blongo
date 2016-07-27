using Blongo.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Blongo.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = UserRoles.Administrator)]
    [Route("admin/users/delete/{id:objectId}", Name = "AdminDeleteUser")]
    public class DeleteUserController : Controller
    {
        public DeleteUserController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpPost]
        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId id, string returnUrl = null)
        {
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.User>(Data.CollectionNames.Users);

            if (await collection.CountAsync(Builders<Data.User>.Filter.Empty) == 1)
            {
                return RedirectToLocal(returnUrl);
            }

            await collection.DeleteOneAsync(Builders<Data.User>.Filter.Where(u => u.Id == id && u.EmailAddress != User.Identity.Name));

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
                return RedirectToRoute("AdminListUsers", new { id = "" });
            }
        }

        private readonly MongoClient _mongoClient;
    }
}