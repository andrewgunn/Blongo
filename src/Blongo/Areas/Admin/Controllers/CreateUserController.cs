namespace Blongo.Areas.Admin.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.CreateUser;
    using MongoDB.Driver;

    [Area("admin")]
    [Authorize]
    [Route("admin/users/create", Name = "AdminCreateUser")]
    public class CreateUserController : Controller
    {
        private readonly MongoClient _mongoClient;

        public CreateUserController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpGet]
        public IActionResult Index(string returnUrl = null)
        {
            var model = new CreateUserModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateUserModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<User>("users");

            if (await collection.CountAsync(Builders<User>.Filter.Where(u => u.EmailAddress == model.EmailAddress)) > 0)
            {
                ModelState.AddModelError(nameof(model.EmailAddress), "There is already a user with that email address");

                return View(model);
            }

            var passwordSalt = Guid.NewGuid().ToString();
            var password = new Password(model.Password, passwordSalt);

            await collection.InsertOneAsync(new User
            {
                Name = model.Name,
                EmailAddress = model.EmailAddress,
                HashedPassword = password.HashedPassword,
                PasswordSalt = passwordSalt,
                Role = model.Role
            });

            return RedirectToLocal(returnUrl);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToRoute("AdminListUsers");
        }
    }
}