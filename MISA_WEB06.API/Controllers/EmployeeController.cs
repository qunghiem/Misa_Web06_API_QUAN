using Microsoft.AspNetCore.Mvc;

namespace MISA_WEB06.API.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
