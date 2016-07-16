using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Threading.Tasks;

namespace Blongo.Controllers
{
    [ResponseCache(Duration = int.MaxValue)]
    [Route("atom-icon-48x48.png", Name = "AtomIcon")]
    public class AtomIconController : Controller
    {
        public AtomIconController(AzureBlobStorage azureBlobStorage)
        {
            _azureBlobStorage = azureBlobStorage;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var fileName = "android-chrome-48x48.png";

            try
            {
                var iconBlob = await _azureBlobStorage.GetBlobAsync(AzureBlobStorageContainers.Icons, fileName);

                await iconBlob.DownloadToStreamAsync(Response.Body);
            }
            catch
            {
                return NotFound();
            }

            string contentType;
            new FileExtensionContentTypeProvider().TryGetContentType(fileName, out contentType);

            if (contentType == null)
            {
                contentType = "application/octet-stream";
            }

            Response.ContentType = contentType;

            return new EmptyResult();
        }

        private readonly AzureBlobStorage _azureBlobStorage;
    }
}
