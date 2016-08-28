namespace Blongo.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Models.CreateFirstUser;
    using MongoDB.Driver;

    [Area("admin")]
    [Route("admin/users/createfirst", Name = "AdminCreateFirstUser")]
    public class CreateFirstUserController : Controller
    {
        private readonly MongoClient _mongoClient;

        public CreateFirstUserController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl = null)
        {
            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<User>(CollectionNames.Users);
            var userCount =
                await collection.CountAsync(Builders<User>.Filter.Where(u => u.Role == UserRoles.Administrator));

            if (userCount > 0)
            {
                return RedirectToLocal(returnUrl);
            }

            var model = new CreateFirstUserModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateFirstUserModel model, string returnUrl = null)
        {
            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<User>("users");
            var userCount =
                await collection.CountAsync(Builders<User>.Filter.Where(u => u.Role == UserRoles.Administrator));

            if (userCount > 0)
            {
                return RedirectToLocal(returnUrl);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var passwordSalt = Guid.NewGuid().ToString();
            var password = new Password(model.Password, passwordSalt);
            var user = new User
            {
                Name = model.Name,
                EmailAddress = model.EmailAddress,
                HashedPassword = password.HashedPassword,
                PasswordSalt = passwordSalt,
                Role = UserRoles.Administrator
            };

            await collection.InsertOneAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.EmailAddress),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "local", ClaimTypes.Name, ClaimTypes.Role);
            await HttpContext.Authentication.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity));

            return RedirectToLocal(returnUrl);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToRoute("AdminListPosts", new {id = ""});
        }
    }
}