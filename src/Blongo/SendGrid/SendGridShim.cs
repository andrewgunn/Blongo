namespace Blongo.SendGrid
{
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using global::SendGrid;

    public class SendGridShim : ISendGridShim
    {
        private readonly SendGridSettings _sendGridSettings;

        public SendGridShim(SendGridSettings sendGridSettings)
        {
            _sendGridSettings = sendGridSettings;
        }

        public async Task SendEmailAsync(MailAddress to, string subject, string body)
        {
            var message = new SendGridMessage
            {
                From = new MailAddress(_sendGridSettings.FromEmailAddress)
            };
            message.AddTo(to.Address);
            message.Subject = subject;
            message.Html = body;
            message.EnableClickTracking(true);

            var credentials = new NetworkCredential(_sendGridSettings.Username, _sendGridSettings.Password);

            var web = new Web(credentials);
            await web.DeliverAsync(message);
        }
    }
}