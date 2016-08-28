namespace Blongo.Areas.Admin.Models.CreateUser
{
    using System.ComponentModel.DataAnnotations;

    public class CreateUserModel
    {
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Please enter an email address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please enter a name")]
        public string Name { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter a password")]
        public string Password { get; set; }

        [Display(Name = "Role")]
        [Required(ErrorMessage = "Please select a role")]
        public string Role { get; set; }
    }
}