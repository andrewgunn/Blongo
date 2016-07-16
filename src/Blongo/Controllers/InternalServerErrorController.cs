using Microsoft.AspNetCore.Mvc;

namespace Blongo.Controllers
{
    [Route("500", Name = "InternalServerError")]
    public class InternalServerErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
