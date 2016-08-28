namespace Blongo.Areas.Admin.Models.UploadFavicon
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;

    public class UploadFaviconModel
    {
        [Required(ErrorMessage = "Please select an image to upload")]
        public IFormFile Image { get; set; }
    }
}