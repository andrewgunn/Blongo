namespace Blongo.Areas.Admin.Controllers
{
    using System;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Models.SendResetPasswordEmail;
    using MongoDB.Driver;
    using SendGrid;

    [Area("admin")]
    [Route("admin/forgotpassword/{emailAddress?}", Name = "AdminSendResetPasswordEmail")]
    public class SendResetPasswordEmailController : Controller
    {
        private readonly MongoClient _mongoClient;
        private readonly ISendGridShim _sendGrid;

        public SendResetPasswordEmailController(MongoClient mongoClient, ISendGridShim sendGrid)
        {
            _mongoClient = mongoClient;
            _sendGrid = sendGrid;
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

            var database = _mongoClient.GetDatabase(DatabaseNames.Blongo);
            var usersCollection = database.GetCollection<User>(CollectionNames.Users);
            var user =
                await usersCollection.Find(Builders<User>.Filter.Where(p => p.EmailAddress == model.EmailAddress))
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

            var resetPasswordLinksCollection =
                database.GetCollection<ResetPasswordLink>(CollectionNames.ResetPasswordLinks);

            var resetPasswordLink = new ResetPasswordLink
            {
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                UserId = user.Id
            };

            await resetPasswordLinksCollection.InsertOneAsync(resetPasswordLink);

            var resetPasswordUrl = Url.RouteUrl("AdminResetPassword", new {id = resetPasswordLink.Id, returnUrl},
                HttpContext.Request.Scheme, HttpContext.Request.Host.Value);

            await
                _sendGrid.SendEmailAsync(new MailAddress(user.EmailAddress), "Forgotten your Blongo password?",
                    $"<a href=\"{resetPasswordUrl}\">{resetPasswordUrl}</a>");

            return RedirectToRoute("AdminResetPasswordInstructions", new {id = resetPasswordLink.Id});
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToRoute("AdminListPosts");
        }
    }
}