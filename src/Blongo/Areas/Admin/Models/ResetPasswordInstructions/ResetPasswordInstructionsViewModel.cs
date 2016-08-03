using System;

namespace Blongo.Areas.Admin.Models.ResetPasswordInstructions
{
    public class ResetPasswordInstructionsViewModel
    {
        public ResetPasswordInstructionsViewModel(DateTime expiresAt)
        {
            ExpiresAt = expiresAt;
        }

        public DateTime ExpiresAt { get; }
    }
}
