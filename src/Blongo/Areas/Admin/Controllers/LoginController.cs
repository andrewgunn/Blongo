namespace Blongo.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Models.Login;
    using MongoDB.Driver;

    [Area("admin")]
    [Route("admin/login", Name = "AdminLogin")]
    public class LoginController : Controller
    {
        private readonly MongoClient _mongoClient;

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

            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<User>(CollectionNames.Users);
            var userCount = await collection.CountAsync(Builders<User>.Filter.Empty);

            if (userCount == 0)
            {
                return new RedirectToRouteResult("AdminCreateFirstUser", new {returnUrl});
            }

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

            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<User>(CollectionNames.Users);
            var user = await collection.Find(Builders<User>.Filter.Where(u => u.EmailAddress == model.EmailAddress))
                .SingleOrDefaultAsync();

            if (user == null)
            {
                return View(model);
            }

            var password = new Password(model.Password, user.PasswordSalt);

            if (password.HashedPassword != user.HashedPassword)
            {
                ModelState.AddModelError("__FORM", "Email address or password are incorrect");

                return View(model);
            }

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
            return RedirectToRoute("AdminListPosts");
        }
    }
}