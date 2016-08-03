using System.ComponentModel.DataAnnotations;

namespace Blongo.Areas.Admin.Models.SendResetPasswordEmail
{
    public class SendResetPasswordEmailModel
    {
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Please enter an email address")]
        public string EmailAddress { get; set; }
    }
}
