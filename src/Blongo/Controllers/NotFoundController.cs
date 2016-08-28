namespace Blongo.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Route("404", Name = "NotFound")]
    public class NotFoundController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}