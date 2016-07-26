using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blongo.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    [Route("admin/image/delete/{fileName}", Name = "AdminDeleteImage")]
    public class UploadDeleteController : Controller
    {
        public UploadDeleteController(AzureBlobStorage azureBlobStorage)
        {
            _azureBlobStorage = azureBlobStorage;
        }

        [HttpPost]
        public async Task<IActionResult> Index(string fileName)
        {
            await _azureBlobStorage.DeleteBlob(AzureBlobStorageContainers.Images, fileName);

            return RedirectToRoute("AdminListImages", new { fileName = "" });
        }

        private readonly AzureBlobStorage _azureBlobStorage;
    }
}