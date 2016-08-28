namespace Blongo.Areas.Admin.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area("admin")]
    [Authorize]
    [Route("admin/image/delete/{fileName}", Name = "AdminDeleteImage")]
    public class UploadDeleteController : Controller
    {
        private readonly AzureBlobStorage _azureBlobStorage;

        public UploadDeleteController(AzureBlobStorage azureBlobStorage)
        {
            _azureBlobStorage = azureBlobStorage;
        }

        [HttpPost]
        public async Task<IActionResult> Index(string fileName)
        {
            await _azureBlobStorage.DeleteBlob(AzureBlobStorageContainers.Images, fileName);

            return RedirectToRoute("AdminListImages", new {fileName = ""});
        }
    }
}