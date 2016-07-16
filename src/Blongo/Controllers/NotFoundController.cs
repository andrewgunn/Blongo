using Blongo.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Blongo.Controllers
{
    [Route("404", Name = "NotFound")]
    public class NotFoundController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
