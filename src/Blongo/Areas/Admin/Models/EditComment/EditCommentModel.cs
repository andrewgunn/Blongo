using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Blongo.Areas.Admin.Models.EditComment
{
    public class EditCommentModel
    {
        [Display(Name = "Body")]
        [DataType(DataType.Html)]
        [Required(ErrorMessage = "Please enter the body")]
        public string Body { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Please enter the email address")]
        public string EmailAddress { get; set; }

        public ObjectId Id { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please enter the name")]
        public string Name { get; set; }

        public ObjectId PostId { get; set; }

        [Display(Name = "Website")]
        public string WebsiteUrl { get; set; }
    }
}
