namespace Blongo.Areas.Admin.Models.ResetPasswordInstructions
{
    using System;

    public class ResetPasswordInstructionsViewModel
    {
        public ResetPasswordInstructionsViewModel(DateTime expiresAt)
        {
            ExpiresAt = expiresAt;
        }

        public DateTime ExpiresAt { get; }
    }
}