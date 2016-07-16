using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Blongo.Models.ViewPost
{
    public class CreateCommentModel
    {
        [Display(Name = "Comments")]
        [DataType(DataType.Html)]
        [Required(ErrorMessage = "Please enter your comments")]
        public string Body { get; set; }

        [Display(Name = "Email address")]
        [RegularExpression(@"^([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})$", ErrorMessage = "Please enter a valid email address")]
        [Required(ErrorMessage = "Please enter your email address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please enter your name")]
        public string Name { get; set; }

        [Display(Name = "Website URL")]
        [RegularExpression(@"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$", ErrorMessage = "Please enter a valid website URL")]
        public string WebsiteUrl { get; set; }
    }
}
