namespace Blongo.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.StaticFiles;

    [ResponseCache(Duration = int.MaxValue)]
    [Route("android-chrome-36x36.png", Name = "AndroidChromex144")]
    [Route("android-chrome-48x48.png", Name = "AndroidChrome48x48")]
    [Route("android-chrome-72x72.png", Name = "AndroidChrome72x72")]
    [Route("android-chrome-96x96.png", Name = "AndroidChrome96x96")]
    [Route("android-chrome-144x144.png", Name = "AndroidChrome144x144")]
    [Route("android-chrome-192x192.png", Name = "AndroidChrome192x192")]
    [Route("apple-touch-icon-57x57.png", Name = "AppleTouchIcon57x57")]
    [Route("apple-touch-icon-60x60.png", Name = "AppleTouchIcon60x60")]
    [Route("apple-touch-icon-72x72.png", Name = "AppleTouchIcon72x72")]
    [Route("apple-touch-icon-76x76.png", Name = "AppleTouchIcon76x76")]
    [Route("apple-touch-icon-114x114.png", Name = "AppleTouchIcon114x114")]
    [Route("apple-touch-icon-120x120.png", Name = "AppleTouchIcon120x120")]
    [Route("apple-touch-icon-144x144.png", Name = "AppleTouchIcon144x144")]
    [Route("apple-touch-icon-152x152.png", Name = "AppleTouchIcon152x152")]
    [Route("apple-touch-icon-180x180.png", Name = "AppleTouchIcon180x180")]
    [Route("apple-touch-icon-precomposed.png", Name = "AppleTouchIconPrecomposed")]
    [Route("apple-touch-icon.png", Name = "AppleTouchIcon")]
    [Route("browserconfig.xml", Name = "BrowserConfiguration")]
    [Route("favicon.ico", Name = "Favicon")]
    [Route("favicon-16x16.png", Name = "Favicon16x16")]
    [Route("favicon-32x32.png", Name = "Favicon32x32")]
    [Route("favicon-96x96.png", Name = "Favicon96x96")]
    [Route("manifest.json", Name = "Manifest")]
    [Route("mstile-70x70.png", Name = "MicrosoftTile70x70")]
    [Route("mstile-144x144.png", Name = "MicrosoftTile144x144")]
    [Route("mstile-150x150.png", Name = "MicrosoftTile150x150")]
    [Route("mstile-310x150.png", Name = "MicrosoftTile310x150")]
    [Route("mstile-310x310.png", Name = "MicrosoftTile310x310")]
    [Route("safari-pinned-tab.svg", Name = "SafariPinnedTab")]
    public class FaviconController : Controller
    {
        private readonly AzureBlobStorage _azureBlobStorage;

        public FaviconController(AzureBlobStorage azureBlobStorage)
        {
            _azureBlobStorage = azureBlobStorage;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var fileName = Request.Path.Value.Substring(1);

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
    }
}