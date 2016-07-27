using Blongo.Areas.Admin.Models.CreateUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Blongo.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    [Route("admin/users/create", Name = "AdminCreateUser")]
    public class CreateUserController : Controller
    {
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

            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.User>("users");

            if (await collection.CountAsync(Builders<Data.User>.Filter.Where(u => u.EmailAddress == model.EmailAddress)) > 0)
            {
                ModelState.AddModelError(nameof(model.EmailAddress), "There is already a user with that email address");

                return View(model);
            }

            var passwordSalt = Guid.NewGuid().ToString();
            var password = new Password(model.Password, passwordSalt);

            await collection.InsertOneAsync(new Data.User
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
            else
            {
                return RedirectToRoute("AdminListUsers");
            }
        }

        private readonly MongoClient _mongoClient;
    }
}
