using Blongo.Areas.Admin.Models.ResetPassword;
using Blongo.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blongo.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/resetpassword/{id:objectId}", Name = "AdminResetPassword")]
    public class ResetPasswordController : Controller
    {
        public ResetPasswordController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId id)
        {
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var resetPasswordLinksCollection = database.GetCollection<Data.ResetPasswordLink>(Data.CollectionNames.ResetPasswordLinks);

            var resetPasswordLink = await resetPasswordLinksCollection.Find(Builders<Data.ResetPasswordLink>.Filter.Where(rpl => rpl.Id == id))
                .SingleOrDefaultAsync();

            if (resetPasswordLink == null || resetPasswordLink.ExpiresAt < DateTime.UtcNow)
            {
                return NotFound();
            }

            var model = new ResetPasswordModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId id, ResetPasswordModel model)
        {
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var resetPasswordLinksCollection = database.GetCollection<Data.ResetPasswordLink>(Data.CollectionNames.ResetPasswordLinks);

            var resetPasswordLink = await resetPasswordLinksCollection.Find(Builders<Data.ResetPasswordLink>.Filter.Where(rpl => rpl.Id == id))
                .SingleOrDefaultAsync();

            if (resetPasswordLink == null || resetPasswordLink.ExpiresAt < DateTime.UtcNow)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usersCollection = database.GetCollection<Data.User>(Data.CollectionNames.Users);
            var user = await usersCollection.Find(Builders<Data.User>.Filter.Where(u => u.Id == resetPasswordLink.UserId))
               .Project(u => new
               {
                   u.EmailAddress,
                   u.PasswordSalt,
                   u.Role
               })
               .SingleOrDefaultAsync();

            var password = new Password(model.Password, user.PasswordSalt);

            var update = Builders<Data.User>.Update
                .Set(u => u.HashedPassword, password.HashedPassword);
            await usersCollection.UpdateOneAsync(Builders<Data.User>.Filter.Where(u => u.Id == id), update);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.EmailAddress),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "local", ClaimTypes.Name, ClaimTypes.Role);
            await HttpContext.Authentication.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity));

            return RedirectToRoute("AdminListPosts");
        }

        private readonly MongoClient _mongoClient;
    }
}
