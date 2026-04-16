using Microsoft.AspNetCore.Mvc;
using MISA.WEB26.Common.Resources;
using MISA_WEB06.BL.Interface;
using MISA_WEB06.Common.Model;

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

        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Lấy bản ghi theo id
        /// </summary
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]  Guid id)
        {
            try
            {
                var res = await _baseBL.GetById(id);
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
        /// <summary>
        /// Thêm bản ghi
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody]  T entity)
        {
            try
            {
                var res = await _baseBL.Insert(entity);
                if (res > 0)
                {
                    //return Ok($"Thêm bản ghi thành công cho bảng {entity}");
                    return Ok(res);
                    //return Created();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {

                return StatusCode(500, new ErrorResult()
                {
                    DevMsg = ex.Message,
                    UserMsg = ResourceVN.Exception,
                    MoreInfo = ex.Data
                });
            }
        }

        /// <summary>
        /// Sửa bản ghi
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody]  T entity)
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
                return StatusCode(500, new ErrorResult()
                {
                    DevMsg = ex.Message,
                    UserMsg = ResourceVN.Exception,
                    MoreInfo = ex.Data
                });
            }
        }

        /// <summary>
        /// Xóa bản ghi theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById([FromRoute]  Guid id)
        {
            try
            {
                var res = await _baseBL.DeleteById(id);
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
        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteMultiple([FromBody] List<Guid> ids)
        {
            try
            {
                var res = await _baseBL.DeleteMultiple(ids);
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

        /// <summary>
        /// Tìm kiếm và phân trang
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm (để trống = lấy tất cả)</param>
        /// <param name="pageNumber">Số trang (bắt đầu từ 1)</param>
        /// <param name="pageSize">Số bản ghi mỗi trang</param>
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? keyword,
            [FromQuery] int pageNumber= 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var res = await _baseBL.Search(keyword, pageNumber, pageSize);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResult
                {
                    DevMsg = ex.Message,
                    UserMsg = ResourceVN.Exception,
                    MoreInfo = ex.Data
                });
            }
        }
    }
}
