using Blongo.Areas.Admin.Models.EditUser;
using Blongo.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
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
                    Role = u.Role,
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
                .Set(u => u.Role, model.Role)
                .Set(u => u.Name, model.Name)
                .Set(u => u.EmailAddress, model.EmailAddress);
            await collection.UpdateOneAsync(Builders<Data.User>.Filter.Where(u => u.Id == id), update);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.EmailAddress),
                new Claim(ClaimTypes.Role, model.Role)
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
                return RedirectToRoute("AdminListUsers", new { id = "" });
            }
        }

        private readonly MongoClient _mongoClient;
    }
}