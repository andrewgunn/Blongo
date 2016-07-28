using Blongo.Areas.Admin.Models.EditUser;
using Blongo.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blongo.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = UserRoles.Administrator)]
    [Route("admin/users/edit/{id:objectId}", Name = "AdminEditUser")]
    public class EditUserController : Controller
    {
        public EditUserController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId id, string returnUrl = null)
        {
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.User>(Data.CollectionNames.Users);
            var model = await collection.Find(Builders<Data.User>.Filter.Where(u => u.Id == id))
                .Project(u => new EditUserModel
                {
                    Id = u.Id,
                    Role = u.EmailAddress != User.Identity.Name ? u.Role : null,
                    Name = u.Name,
                    EmailAddress = u.EmailAddress,
                })
                .SingleOrDefaultAsync();

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index([ModelBinder(BinderType = typeof(ObjectIdModelBinder))] ObjectId id, EditUserModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                model.Id = id;

                return View(model);
            }

            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.User>(Data.CollectionNames.Users);
            var update = Builders<Data.User>.Update
                .Set(u => u.Name, model.Name)
                .Set(u => u.EmailAddress, model.EmailAddress);

            var claims = new List<Claim>(User.Claims);

            var nameClaim = User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Name);

            if (nameClaim != null)
            {
                claims.Remove(nameClaim);
            }

            claims.Add(new Claim(ClaimTypes.Name, model.EmailAddress));

            if (User.IsInRole(UserRoles.Administrator) && await collection.CountAsync(Builders<Data.User>.Filter.Where(u => u.Id == id && u.EmailAddress == User.Identity.Name)) == 0)
            {
                if (string.IsNullOrEmpty(model.Role))
                {
                    ModelState.AddModelError(nameof(model.Role), "Please enter a role");

                    return View(model);
                }

                // You can't change a user's role unless you're an Administrator.
                // You can't change your own role, or demote yourself - there always has to be at least one Administrator.
                update.Set(u => u.Role, model.Role);

                var roleClaim = User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Role);

                if (roleClaim != null)
                {
                    claims.Remove(roleClaim);
                }

                claims.Add(new Claim(ClaimTypes.Role, model.Role));
            }

            await collection.UpdateOneAsync(Builders<Data.User>.Filter.Where(u => u.Id == id), update);

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
                return RedirectToRoute("AdminListUsers", new { id = "" });
            }
        }

        private readonly MongoClient _mongoClient;
    }
}