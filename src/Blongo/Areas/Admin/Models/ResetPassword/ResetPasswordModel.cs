namespace Blongo.Areas.Admin.Models.ResetPassword
{
    using System.ComponentModel.DataAnnotations;

    public class ResetPasswordModel
    {
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter a password")]
        public string Password { get; set; }
    }
}