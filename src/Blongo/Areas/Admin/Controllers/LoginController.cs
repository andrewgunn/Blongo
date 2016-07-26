using Blongo.Areas.Admin.Models.Login;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blongo.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/login", Name = "AdminLogin")] 
    public class LoginController : Controller
    {
        public LoginController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToLocal(returnUrl);
            }

            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.User>(Data.CollectionNames.Users);
            var userCount = await collection.CountAsync(Builders<Data.User>.Filter.Empty);

            if (userCount == 0)
            {
                return new RedirectToRouteResult("AdminCreateFirstUser", new { returnUrl });
            }

            ViewData["ReturnUrl"] = returnUrl;

            var model = new LoginModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginModel model, string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToLocal(returnUrl);
            }

            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.User>(Data.CollectionNames.Users);
            var user = await collection.Find(Builders<Data.User>.Filter.Where(u => u.EmailAddress == model.EmailAddress))
                .SingleOrDefaultAsync();

            if (user == null)
            {
                ViewData["ReturnUrl"] = returnUrl;

                return View(model);
            }

            var password = new Password(model.Password, user.PasswordSalt);

            if (password.HashedPassword != user.HashedPassword)
            {
                ViewData["ReturnUrl"] = returnUrl;

                return View(model);
            }

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
                return RedirectToRoute("AdminListPosts");
            }
        }

        private readonly MongoClient _mongoClient;
    }
}
