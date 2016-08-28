namespace Blongo.Areas.Admin.Controllers
{
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using ModelBinding;
    using Models.ChangeUserPassword;
    using MongoDB.Bson;
    using MongoDB.Driver;

    [Area("admin")]
    [Authorize(Roles = UserRoles.Administrator)]
    [Route("admin/users/changepassword/{id:objectId}", Name = "AdminChangeUserPassword")]
    public class ChangeUserPasswordController : Controller
    {
        private readonly MongoClient _mongoClient;

        public ChangeUserPasswordController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId id,
            string returnUrl = null)
        {
            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<User>(CollectionNames.Users);
            var userCount = await collection.CountAsync(Builders<User>.Filter.Where(u => u.Id == id));

            if (userCount == 0)
            {
                return NotFound();
            }

            var model = new ChangeUserPasswordModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId id,
            ChangeUserPasswordModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<User>(CollectionNames.Users);
            var user = await collection.Find(Builders<User>.Filter.Where(u => u.Id == id))
                .Project(u => new
                {
                    u.PasswordSalt
                })
                .SingleOrDefaultAsync();

            var password = new Password(model.Password, user.PasswordSalt);

            var update = Builders<User>.Update
                .Set(u => u.HashedPassword, password.HashedPassword);
            await collection.UpdateOneAsync(Builders<User>.Filter.Where(u => u.Id == id), update);

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