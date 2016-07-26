using Blongo.Areas.Admin.Models.EditSettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Blongo.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    [Route("admin/settings", Name = "AdminEditSettings")]
    public class EditSettingsController : Controller
    {
        public EditSettingsController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.Blog>(Data.CollectionNames.Blogs);
            var model = await collection.Find(Builders<Data.Blog>.Filter.Empty)
                .Project(b => new EditSettingsModel
                {
                    Name = b.Name,
                    Description = b.Description,
                    FeedUrl = b.FeedUrl,
                    AzureStorageConnectionString = b.AzureStorageConnectionString,
                    AkismetApiKey = b.AkismetApiKey,
                    RealFaviconGeneratorApiKey = b.RealFaviconGeneratorApiKey,
                    Styles = b.Styles,
                    Scripts = b.Scripts
                })
                .SingleOrDefaultAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(EditSettingsModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var collection = database.GetCollection<Data.Blog>(Data.CollectionNames.Blogs);
            var update = Builders<Data.Blog>.Update
                .Set(b => b.Name, model.Name)
                .Set(b => b.Description, model.Description)
                .Set(b => b.FeedUrl, model.FeedUrl)
                .Set(b => b.AzureStorageConnectionString, model.AzureStorageConnectionString)
                .Set(b => b.AkismetApiKey, model.AkismetApiKey)
                .Set(b => b.RealFaviconGeneratorApiKey, model.RealFaviconGeneratorApiKey)
                .Set(b => b.Styles, model.Styles)
                .Set(b => b.Scripts, model.Scripts);
            await collection.UpdateOneAsync(Builders<Data.Blog>.Filter.Empty, update, new UpdateOptions { IsUpsert = true });

            return RedirectToRoute("AdminListPosts");
        }

        private readonly MongoClient _mongoClient;
    }
}