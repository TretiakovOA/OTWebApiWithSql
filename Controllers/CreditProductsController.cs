using Microsoft.AspNetCore.Mvc;
using OTWebApiWithSql.Interfaces.Services;
using OTWebApiWithSql.Models;

namespace OTWebApiWithSql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditProductsController : ControllerBase
    {
        private readonly ICreditProductService _service;

        public CreditProductsController(ICreditProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<PaginatedResult<CreditProduct>> GetProducts(
            [FromQuery] int page = 1,
            [FromQuery] int size = 10)
        {
            try
            {
                return Ok(_service.GetProducts(page, size));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<CreditProduct> Get(int id)
        {
            try
            {
                var product = _service.GetProduct(id);
                return product != null ? Ok(product) : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreditApplication), 201)]
        public IActionResult Create([FromBody] CreditProduct product)
        {
            try
            {
                var id = _service.CreateProduct(product);
                return CreatedAtAction(nameof(Get), new { id }, product);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CreditProduct product)
        {
            try
            {
                if (id != product.Id)
                    return BadRequest(new { Error = "ID in URL and body mismatch" });

                _service.UpdateProduct(product);
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
                _service.DeleteProduct(id);
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
