using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Blongo.Areas.Admin.Models.UploadImage
{
    public class UploadImageModel
    {
        [Display(Name = "File name")]
        [Required(ErrorMessage = "Please enter the file name")]
        [RegularExpression(@"[^\.]+.(bmp|gif|ico|jpg|jpeg|png|tiff)", ErrorMessage = "Please enter a valid file name")]
        public string FileName { get; set; }

        [Required(ErrorMessage = "Please select an image to upload")]
        public IFormFile Image { get; set; }
    }
}
