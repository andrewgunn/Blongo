﻿namespace Blongo.Areas.Admin.Controllers
{
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.UploadFavicon;
    using MongoDB.Driver;
    using RealFaviconGeneratorSdk;

    [Area("admin")]
    [Authorize]
    [Route("admin/favicon/upload", Name = "AdminUploadFavicon")]
    public class UploadFaviconController : Controller
    {
        private readonly AzureBlobStorage _azureBlobStorage;

        private readonly HttpClient _httpClient;
        private readonly MongoClient _mongoClient;
        private readonly RealFaviconGenerator _realFaviconGenerator;

        public UploadFaviconController(RealFaviconGenerator realFaviconGenerator, HttpClient httpClient,
            AzureBlobStorage azureBlobStorage, MongoClient mongoClient)
        {
            _realFaviconGenerator = realFaviconGenerator;
            _httpClient = httpClient;
            _azureBlobStorage = azureBlobStorage;
            _mongoClient = mongoClient;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new UploadFaviconModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(UploadFaviconModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            GenerateFaviconsResult faviconResult;

            using (var imageStream = model.Image.OpenReadStream())
            {
                var image = Image.FromStream(imageStream);

                if (image == null)
                {
                    return View(model);
                }

                imageStream.Position = 0;

                faviconResult = await _realFaviconGenerator.GenerateFaviconsAsync(image, GenerateFaviconVersion());
            }

            await _azureBlobStorage.Purge(AzureBlobStorageContainers.Icons);

            foreach (var fileUrl in faviconResult.FileUrls)
            {
                using (var response = await _httpClient.GetAsync(fileUrl))
                {
                    response.EnsureSuccessStatusCode();

                    using (var fileStream = await response.Content.ReadAsStreamAsync())
                    {
                        await
                            _azureBlobStorage.SaveBlobAsync(AzureBlobStorageContainers.Icons, fileStream,
                                fileUrl.AbsolutePath.Split('/').Last());
                    }
                }
            }

            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var collection = database.GetCollection<Blog>(CollectionNames.Blogs);
            var update = Builders<Blog>.Update
                .Set(b => b.FaviconsHtml, faviconResult.Html);
            await collection.UpdateOneAsync(Builders<Blog>.Filter.Empty, update, new UpdateOptions {IsUpsert = true});

            return RedirectToRoute("AdminListPosts");
        }

        private static string GenerateFaviconVersion()
        {
            var guid = Guid.NewGuid();
            var guidString = Convert.ToBase64String(guid.ToByteArray());
            guidString = guidString.Replace("=", "");
            guidString = guidString.Replace("+", "");

            return new UrlSlug(guidString).Value;
        }
    }
}