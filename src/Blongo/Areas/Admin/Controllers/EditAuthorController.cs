using Blongo.Areas.Admin.Models.EditAuthor;
using Blongo.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Blongo.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    [Route("admin/author", Name = "AdminEditAuthor")]
    [ServiceFilter(typeof(UserDataFilter))]
    public class EditAuthorController : Controller
    {
        public EditAuthorController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.Blog>(Data.CollectionNames.Blogs);
            var model = await collection.Find(Builders<Data.Blog>.Filter.Empty)
                .Project(b => new EditAuthorModel
                {
                    Name = b.Author.Name,
                    EmailAddress = b.Author.EmailAddress,
                    GitHubUsername = b.Author.GitHubUsername,
                    TwitterUsername = b.Author.TwitterUsername,
                    WebsiteUrl = b.Author.WebsiteUrl
                })
                .SingleOrDefaultAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(EditAuthorModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.Blog>(Data.CollectionNames.Blogs);
            var update = Builders<Data.Blog>.Update
                .Set(b => b.Author.Name, model.Name)
                .Set(b => b.Author.EmailAddress, model.EmailAddress)
                .Set(b => b.Author.GitHubUsername, model.GitHubUsername)
                .Set(b => b.Author.TwitterUsername, model.TwitterUsername)
                .Set(b => b.Author.WebsiteUrl, model.WebsiteUrl);
            await collection.UpdateOneAsync(Builders<Data.Blog>.Filter.Empty, update, new UpdateOptions { IsUpsert = true });

            return RedirectToRoute("AdminListPosts");
        }

        private readonly MongoClient _mongoClient;
    }
}