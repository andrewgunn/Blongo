namespace Blongo.Areas.Admin.Models.EditUser
{
    using System.ComponentModel.DataAnnotations;
    using MongoDB.Bson;

    public class EditUserModel
    {
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Please enter an email address")]
        public string EmailAddress { get; set; }

        public ObjectId Id { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please enter a name")]
        public string Name { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }
    }
}