namespace Blongo.SendGrid
{
    using System.Net.Mail;
    using System.Threading.Tasks;

    public interface ISendGridShim
    {
        Task SendEmailAsync(MailAddress to, string subject, string body);
    }
}