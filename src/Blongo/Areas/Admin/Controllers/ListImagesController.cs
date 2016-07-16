using Blongo.Areas.Admin.Models.ListImages;
using Blongo.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Blongo.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    [Route("admin/images/{fileName?}", Name = "AdminListImages")]
    [ServiceFilter(typeof(UserDataFilter))]
    public class ListImagesController : Controller
    {
        public ListImagesController(AzureBlobStorage azureBlobStorage)
        {
            _azureBlobStorage = azureBlobStorage;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string fileName = null, [FromQuery(Name = "p")] int pageNumber = 1)
        {
            if (pageNumber < 1)
            {
                return NotFound();
            }

            const int pageSize = 10;
            var imageBlobs = await _azureBlobStorage.GetBlobsAsync(AzureBlobStorageContainers.Images, fileName);
            var totalCount = imageBlobs.Count();
            var maximumPageNumber = Math.Max(1, (int)Math.Ceiling((double)totalCount / pageSize));

            if (totalCount > 0)
            {
                imageBlobs = imageBlobs.Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }

            var paging = new Paging(pageNumber, pageSize, maximumPageNumber);
            var model = new ListImagesViewModel(imageBlobs.Select(b => b.Uri).ToList(), paging);

            return View(model);
        }

        private readonly AzureBlobStorage _azureBlobStorage;
    }
}