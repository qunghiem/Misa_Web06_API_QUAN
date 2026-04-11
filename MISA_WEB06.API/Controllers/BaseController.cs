using Microsoft.AspNetCore.Mvc;
using MISA_WEB06.BL.Interface;

namespace MISA_WEB06.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<T> : ControllerBase
    {
        protected IBaseBL<T> _baseBL;

        public BaseController(IBaseBL<T> baseBL)
        {
            _baseBL = baseBL;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var res = await _baseBL.GetAll();

                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetById(Guid employeeId)
        {
            try
            {
                var res = await _baseBL.GetById(employeeId);
                if (res != null)
                {
                    return Ok(res);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insert(T entity)
        {
            try
            {
                var res = await _baseBL.Insert(entity);
                if (res > 0)
                {
                    return Ok(res);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(T entity)
        {
            try
            {
                var res = await _baseBL.Update(entity);
                if (res > 0)
                {
                    return Ok(res);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> DeleteById(Guid employeeId)
        {
            try
            {
                var res = await _baseBL.DeleteById(employeeId);
                if (res > 0)
                {
                    return Ok(res);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
