using Microsoft.AspNetCore.Mvc;
using OTWebApiWithSql.Interfaces.Services;
using OTWebApiWithSql.Models;

namespace OTWebApiWithSql.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet]
    public ActionResult<PaginatedResult<Client>> GetClients(
        [FromQuery] int page = 1,
        [FromQuery] int size = 10)
    {
        try
        {
            return Ok(_clientService.GetClients(page, size));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{id}")]
    public ActionResult<Client> Get(int id)
    {
        var client = _clientService.GetClient(id);
        return client == null ? NotFound() : Ok(client);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Client), 201)]
    public IActionResult Create([FromBody] Client client)
    {
        try
        {
            var id = _clientService.CreateClient(client);
            return CreatedAtAction(nameof(Get), new { id }, client);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Client client)
    {
        try
        {
            if (id != client.Id) return BadRequest("ID mismatch");
            _clientService.UpdateClient(client);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            _clientService.DeleteClient(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
