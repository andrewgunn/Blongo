using System.ComponentModel.DataAnnotations;

namespace Blongo.Areas.Admin.Models.Login
{
    public class LoginModel
    {
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Please enter your email address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; }
    }
}
