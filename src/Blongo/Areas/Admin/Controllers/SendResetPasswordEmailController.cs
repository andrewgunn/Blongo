using Blongo.Areas.Admin.Models.SendResetPasswordEmail;
using Blongo.Config;
using Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SendGrid;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Blongo.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/forgotpassword", Name = "AdminSendResetPasswordEmail")]
    public class SendResetPasswordEmailController : Controller
    {
        public SendResetPasswordEmailController(MongoClient mongoClient, IOptions<SendGridConfig> sendGridConfig)
        {
            _mongoClient = mongoClient;
            _sendGridConfig = sendGridConfig.Value;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new SendResetPasswordEmailModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(SendResetPasswordEmailModel model)
        {
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

            var resetPasswordUrl = Url.RouteUrl("AdminResetPassword", new { id = resetPasswordLink.Id }, HttpContext.Request.Scheme, HttpContext.Request.Host.Value);

            var message = new SendGridMessage();
            message.From = new MailAddress(_sendGridConfig.From);
            message.AddTo($"{user.Name} <{user.EmailAddress}>");
            message.Subject = "Forgotten your Blongo password?";
            message.Html = $"<a href=\"{resetPasswordUrl}\">{resetPasswordUrl}</a>";
            message.EnableClickTracking(true);

            var credentials = new NetworkCredential(_sendGridConfig.Username, _sendGridConfig.Password);

            var web = new Web(credentials);
            try
            {
                await web.DeliverAsync(message);
            }
            catch(InvalidApiRequestException exception)
            {
                return Json(exception.Errors);
            }

            return RedirectToRoute("AdminResetPasswordInstructions", new { id = resetPasswordLink.Id });
        }

        private readonly SendGridConfig _sendGridConfig;
        private readonly MongoClient _mongoClient;
    }
}
