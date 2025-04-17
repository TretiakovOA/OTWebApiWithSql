using Microsoft.AspNetCore.Mvc;
using OTWebApiWithSql.Interfaces.Services;
using OTWebApiWithSql.Models;

namespace OTWebApiWithSql.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OperatorsController : ControllerBase
{
    private readonly IOperatorService _service;

    public OperatorsController(IOperatorService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<PaginatedResult<Operator>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int size = 10)
    {
        try
        {
            return Ok(_service.GetOperators(page, size));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public ActionResult<Operator> GetById(int id)
    {
        try
        {
            var entity = _service.GetOperator(id);
            return entity != null ? Ok(entity) : NotFound();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(Operator), 201)]
    public IActionResult Create([FromBody] Operator entity)
    {
        try
        {
            var id = _service.CreateOperator(entity);
            return CreatedAtAction(nameof(GetById), new { id }, entity);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Operator entity)
    {
        try
        {
            if (id != entity.Id)
                return BadRequest(new { Error = "ID mismatch" });

            _service.UpdateOperator(entity);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
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
            _service.DeleteOperator(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }
}
