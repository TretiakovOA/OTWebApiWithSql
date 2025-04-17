using Microsoft.AspNetCore.Mvc;
using OTWebApiWithSql.Interfaces.Services;
using OTWebApiWithSql.Models;

namespace OTWebApiWithSql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallsController : ControllerBase
    {
        private readonly ICallService _service;

        public CallsController(ICallService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<PaginatedResult<Call>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int size = 10)
        {
            try
            {
                return Ok(_service.GetCalls(page, size));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Call> Get(int id)
        {
            try
            {
                var call = _service.GetCall(id);
                return call != null ? Ok(call) : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateResult(int id, string callResult)
        {
            try
            {
                _service.UpdateCallDetails(id, callResult);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _service.DeleteCall(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}
