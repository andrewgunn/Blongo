using Blongo.Areas.Admin.Models.CreateFirstUser;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blongo.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/users/createfirst", Name = "AdminCreateFirstUser")]
    public class CreateFirstUserController : Controller
    {
        public CreateFirstUserController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl = null)
        {
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.User>(Data.CollectionNames.Users);
            var userCount = await collection.CountAsync(Builders<Data.User>.Filter.Where(u => u.Role == UserRoles.Administrator));

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
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.User>("users");
            var userCount = await collection.CountAsync(Builders<Data.User>.Filter.Where(u => u.Role == UserRoles.Administrator));

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
            var user = new Data.User
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
                    new Claim(ClaimTypes.Role, user.Role.ToString())
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
            else
            {
                return RedirectToRoute("AdminListPosts", new { id = "" });
            }
        }

        private readonly MongoClient _mongoClient;
    }
}
