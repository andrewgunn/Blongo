namespace Blongo.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using ModelBinding;
    using Models.ResetPassword;
    using MongoDB.Bson;
    using MongoDB.Driver;

    [Area("admin")]
    [Route("admin/resetpassword/{id:objectId}", Name = "AdminResetPassword")]
    public class ResetPasswordController : Controller
    {
        private readonly MongoClient _mongoClient;

        public ResetPasswordController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId id)
        {
            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var resetPasswordLinksCollection =
                database.GetCollection<ResetPasswordLink>(CollectionNames.ResetPasswordLinks);

            var resetPasswordLink =
                await resetPasswordLinksCollection.Find(Builders<ResetPasswordLink>.Filter.Where(rpl => rpl.Id == id))
                    .SingleOrDefaultAsync();

            if (resetPasswordLink == null || resetPasswordLink.ExpiresAt < DateTime.UtcNow)
            {
                return NotFound();
            }

            var model = new ResetPasswordModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId id,
            ResetPasswordModel model, string returnUrl = null)
        {
            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var resetPasswordLinksCollection =
                database.GetCollection<ResetPasswordLink>(CollectionNames.ResetPasswordLinks);
            var resetPasswordLink =
                await resetPasswordLinksCollection.Find(Builders<ResetPasswordLink>.Filter.Where(rpl => rpl.Id == id))
                    .SingleOrDefaultAsync();

            if (resetPasswordLink == null || resetPasswordLink.ExpiresAt < DateTime.UtcNow)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usersCollection = database.GetCollection<User>(CollectionNames.Users);
            var user = await usersCollection.Find(Builders<User>.Filter.Where(u => u.Id == resetPasswordLink.UserId))
                .Project(u => new
                {
                    u.EmailAddress,
                    u.PasswordSalt,
                    u.Role
                })
                .SingleOrDefaultAsync();

            var password = new Password(model.Password, user.PasswordSalt);

            var update = Builders<User>.Update
                .Set(u => u.HashedPassword, password.HashedPassword);
            await usersCollection.UpdateOneAsync(Builders<User>.Filter.Where(u => u.Id == id), update);

            await
                resetPasswordLinksCollection.DeleteOneAsync(
                    Builders<ResetPasswordLink>.Filter.Where(rpl => rpl.Id == resetPasswordLink.Id));

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