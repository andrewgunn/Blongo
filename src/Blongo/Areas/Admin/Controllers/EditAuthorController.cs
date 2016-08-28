namespace Blongo.Areas.Admin.Controllers
{
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.EditAuthor;
    using MongoDB.Driver;

    [Area("admin")]
    [Authorize]
    [Route("admin/author", Name = "AdminEditAuthor")]
    public class EditAuthorController : Controller
    {
        private readonly MongoClient _mongoClient;

        public EditAuthorController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<Blog>(CollectionNames.Blogs);
            var model = await collection.Find(Builders<Blog>.Filter.Empty)
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

            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<Blog>(CollectionNames.Blogs);
            var update = Builders<Blog>.Update
                .Set(b => b.Author.Name, model.Name)
                .Set(b => b.Author.EmailAddress, model.EmailAddress)
                .Set(b => b.Author.GitHubUsername, model.GitHubUsername)
                .Set(b => b.Author.TwitterUsername, model.TwitterUsername)
                .Set(b => b.Author.WebsiteUrl, model.WebsiteUrl);
            await collection.UpdateOneAsync(Builders<Blog>.Filter.Empty, update, new UpdateOptions {IsUpsert = true});

            return RedirectToRoute("AdminListPosts");
        }
    }
}