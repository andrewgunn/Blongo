using Blongo.Data;
using System.ComponentModel.DataAnnotations;

namespace Blongo.Areas.Admin.Models.CreateUser
{
    public class CreateUserModel
    {
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Please enter the email address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter the password")]
        public string Password { get; set; }

        [Display(Name = "Role")]
        [Required(ErrorMessage = "Please select the role")]
        public UserRoles Role { get; set; }
    }
}
