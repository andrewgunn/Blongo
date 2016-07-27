using Blongo.Areas.Admin.Models.ChangeUserPassword;
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
    [Route("admin/users/changepassword/{id:objectId}", Name = "AdminChangeUserPassword")]
    public class ChangeUserPasswordController : Controller
    {
        public ChangeUserPasswordController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId id, string returnUrl = null)
        {
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.User>(Data.CollectionNames.Users);
            var user = await collection.Find(Builders<Data.User>.Filter.Where(u => u.Id == id))
                .Project(u => new { })
                .SingleOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            var model = new ChangeUserPasswordModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId id, ChangeUserPasswordModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.User>(Data.CollectionNames.Users);
            var user = await collection.Find(Builders<Data.User>.Filter.Where(u => u.Id == id))
                .Project(u => new
                {
                    u.PasswordSalt
                })
                .SingleOrDefaultAsync();

            var password = new Password(model.Password, user.PasswordSalt);

            var update = Builders<Data.User>.Update
                .Set(u => u.HashedPassword, password.HashedPassword);
            await collection.UpdateOneAsync(Builders<Data.User>.Filter.Where(u => u.Id == id), update);

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