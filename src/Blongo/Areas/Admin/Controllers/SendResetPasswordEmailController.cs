using Blongo.Areas.Admin.Models.SendResetPasswordEmail;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SendGrid;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Blongo.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/forgotpassword/{emailAddress?}", Name = "AdminSendResetPasswordEmail")]
    public class SendResetPasswordEmailController : Controller
    {
        public SendResetPasswordEmailController(SendGridSettings sendGridSettings, MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
            _sendGridSettings = sendGridSettings;
        }

        [HttpGet]
        public IActionResult Index(string emailAddress)
        {
            var model = new SendResetPasswordEmailModel
            {
                EmailAddress = emailAddress
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(SendResetPasswordEmailModel model, string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToLocal(returnUrl);
            }

            var database = _mongoClient.GetDatabase(Data.DatabaseNames.Blongo);
            var usersCollection = database.GetCollection<Data.User>(Data.CollectionNames.Users);
            var user = await usersCollection.Find(Builders<Data.User>.Filter.Where(p => p.EmailAddress == model.EmailAddress))
                .Project(u => new
                {
                    u.Id,
                    u.Name,
                    u.EmailAddress
                })
                .SingleOrDefaultAsync();

            if (user == null)
            {
                ModelState.AddModelError(nameof(model.EmailAddress), "Email address not found");

                return View(model);
            }

            var resetPasswordLinksCollection = database.GetCollection<Data.ResetPasswordLink>(Data.CollectionNames.ResetPasswordLinks);

            var resetPasswordLink = new Data.ResetPasswordLink
            {
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                UserId = user.Id
            };

            await resetPasswordLinksCollection.InsertOneAsync(resetPasswordLink);

            var resetPasswordUrl = Url.RouteUrl("AdminResetPassword", new { id = resetPasswordLink.Id, returnUrl }, HttpContext.Request.Scheme, HttpContext.Request.Host.Value);

            var message = new SendGridMessage();
            message.From = new MailAddress(_sendGridSettings.FromEmailAddress);
            message.AddTo($"{user.Name} <{user.EmailAddress}>");
            message.Subject = "Forgotten your Blongo password?";
            message.Html = $"<a href=\"{resetPasswordUrl}\">{resetPasswordUrl}</a>";
            message.EnableClickTracking(true);

            var credentials = new NetworkCredential(_sendGridSettings.Username, _sendGridSettings.Password);

            var web = new Web(credentials);
            await web.DeliverAsync(message);

            return RedirectToRoute("AdminResetPasswordInstructions", new { id = resetPasswordLink.Id });
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToRoute("AdminListPosts");
            }
        }

        private readonly MongoClient _mongoClient;
        private readonly SendGridSettings _sendGridSettings;
    }
}
