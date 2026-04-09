using Microsoft.AspNetCore.Mvc;

namespace MISA_WEB06.API.Controllers
{
    public class BaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
