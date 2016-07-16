using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Blongo.Areas.Admin.Models.UploadFavicon
{
    public class UploadFaviconModel
    {
        [Required(ErrorMessage = "Please select an image to upload")]
        public IFormFile Image { get; set; }
    }
}
