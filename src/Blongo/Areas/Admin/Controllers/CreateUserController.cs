using Blongo.Areas.Admin.Models.CreateUser;
using Blongo.Filters;
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
    [ServiceFilter(typeof(UserDataFilter))]
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
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.User>("users");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userExists = await collection.CountAsync(Builders<Data.User>.Filter.Where(u => u.EmailAddress == model.EmailAddress)) > 0;

            if (userExists)
            {
                return View(model);
            }

            var passwordSalt = Guid.NewGuid().ToString();
            var password = new Password(model.Password, passwordSalt);
            var user = new Data.User
            {
                EmailAddress = model.EmailAddress,
                HashedPassword = password.HashedPassword,
                PasswordSalt = passwordSalt,
                Role = model.Role
            };

            await collection.InsertOneAsync(user);

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
                return RedirectToRoute("AdminListPosts");
            }
        }

        private readonly MongoClient _mongoClient;
    }
}
