namespace Blongo.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Route("500", Name = "InternalServerError")]
    public class InternalServerErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}