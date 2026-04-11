using Microsoft.AspNetCore.Mvc;
using MISA_WEB06.BL.Interface;
using MISA_WEB06.Common.Model;

namespace MISA_WEB06.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeeController : BaseController<Employee>
    {
        public EmployeeController(IBaseBL<Employee> baseBL) : base(baseBL)
        {
        }
    }
}
