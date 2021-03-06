﻿namespace Blongo.Areas.Admin.Controllers
{
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.EditSettings;
    using MongoDB.Driver;

    [Area("admin")]
    [Authorize]
    [Route("admin/settings", Name = "AdminEditSettings")]
    public class EditSettingsController : Controller
    {
        private readonly MongoClient _mongoClient;

        public EditSettingsController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<Blog>(CollectionNames.Blogs);
            var model = await collection.Find(Builders<Blog>.Filter.Empty)
                .Project(b => new EditSettingsModel
                {
                    Name = b.Name,
                    Description = b.Description,
                    CompanyName = b.Company == null ? null : b.Company.Name,
                    CompanyWebsiteUrl = b.Company == null ? null : b.Company.WebsiteUrl,
                    FeedUrl = b.FeedUrl,
                    AzureStorageConnectionString = b.AzureStorageConnectionString,
                    SendGridFromEmailAddress = b.SendGridSettings == null ? null : b.SendGridSettings.FromEmailAddress,
                    SendGridUsername = b.SendGridSettings == null ? null : b.SendGridSettings.Username,
                    SendGridPassword = b.SendGridSettings == null ? null : b.SendGridSettings.Password,
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

            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<Blog>(CollectionNames.Blogs);
            var update = Builders<Blog>.Update
                .Set(b => b.Name, model.Name)
                .Set(b => b.Description, model.Description)
                .Set(b => b.Company, new Company
                {
                    Name = model.CompanyName,
                    WebsiteUrl = model.CompanyWebsiteUrl
                })
                .Set(b => b.FeedUrl, model.FeedUrl)
                .Set(b => b.AzureStorageConnectionString, model.AzureStorageConnectionString)
                .Set(b => b.SendGridSettings, new SendGridSettings
                {
                    FromEmailAddress = model.SendGridFromEmailAddress,
                    Username = model.SendGridUsername,
                    Password = model.SendGridPassword
                })
                .Set(b => b.AkismetApiKey, model.AkismetApiKey)
                .Set(b => b.RealFaviconGeneratorApiKey, model.RealFaviconGeneratorApiKey)
                .Set(b => b.Styles, model.Styles)
                .Set(b => b.Scripts, model.Scripts);
            await collection.UpdateOneAsync(Builders<Blog>.Filter.Empty, update, new UpdateOptions {IsUpsert = true});

            return RedirectToRoute("AdminListPosts");
        }
    }
}