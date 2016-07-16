using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blongo.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    [Route("admin/logout", Name = "AdminLogout")]
    public class LogoutController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Index(string returnUrl = null)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToLocal(returnUrl);
            }

            await HttpContext.Authentication.SignOutAsync("Cookies");

            return RedirectToLocal(returnUrl);
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
    }
}
