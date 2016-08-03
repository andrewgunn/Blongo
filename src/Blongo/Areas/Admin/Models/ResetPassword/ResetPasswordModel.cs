using System.ComponentModel.DataAnnotations;

namespace Blongo.Areas.Admin.Models.ResetPassword
{
    public class ResetPasswordModel
    {
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter a password")]
        public string Password { get; set; }
    }
}
