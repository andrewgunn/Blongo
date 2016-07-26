using System.ComponentModel.DataAnnotations;

namespace Blongo.Areas.Admin.Models.CreateFirstUser
{
    public class CreateFirstUserModel
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
    }
}
