using Microsoft.AspNetCore.Mvc;
using OTWebApiWithSql.Interfaces.Services;
using OTWebApiWithSql.Models;

namespace OTWebApiWithSql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditApplicationsController : ControllerBase
    {
        private readonly ICreditApplicationService _service;

        public CreditApplicationsController(ICreditApplicationService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<PaginatedResult<CreditApplication>> GetApplications(
            [FromQuery] int page = 1,
            [FromQuery] int size = 10)
        {
            return Ok(_service.GetApplications(page, size));
        }

        [HttpGet("{id}")]
        public ActionResult<CreditApplication> Get(int id)
        {
            try
            {
                var application = _service.GetApplication(id);
                return application != null ? Ok(application) : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreditApplication), 201)]
        public IActionResult Create([FromBody] CreditApplication application)
        {
            try
            {
                var id = _service.CreateApplication(application);
                return CreatedAtAction(nameof(Get), new { id }, application);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CreditApplication application)
        {
            try
            {
                if (id != application.Id)
                    return BadRequest(new { Error = "ID in URL and body mismatch" });

                _service.UpdateApplication(application);
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
                _service.DeleteApplication(id);
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
