using Blongo.Areas.Admin.Models.UploadImage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace Blongo.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    [Route("admin/image/upload", Name = "AdminUploadImage")]
    public class UploadImageController : Controller
    {
        public UploadImageController(AzureBlobStorage azureBlobStorage)
        {
            _azureBlobStorage = azureBlobStorage;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new UploadImageModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(UploadImageModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var imageStream = model.Image.OpenReadStream())
            using (var memoryStream = new MemoryStream((int)model.Image.Length))
            {
                var image = Image.FromStream(imageStream);

                if (image == null)
                {
                    return View(model);
                }

                imageStream.Position = 0;

                await imageStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                var fileName = new UrlSlug(model.FileName).Value;

                await _azureBlobStorage.SaveBlobAsync(AzureBlobStorageContainers.Images, memoryStream, fileName);
            }

            return RedirectToRoute("AdminListImages");
        }

        private readonly AzureBlobStorage _azureBlobStorage;
    }
}