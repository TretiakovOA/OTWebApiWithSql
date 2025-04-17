using Microsoft.AspNetCore.Mvc;
using OTWebApiWithSql.Interfaces.Services;

namespace OTWebApiWithSql.Controllers
{
    [Route("api/calls/{callId}/operators")]
    [ApiController]
    public class CallOperatorsController : ControllerBase
    {
        private readonly ICallService _callService;

        public CallOperatorsController(ICallService callService)
        {
            _callService = callService;
        }

        [HttpPost("{operatorId}")]
        public IActionResult AssignOperator(int callId, int operatorId)
        {
            try
            {
                _callService.AssignOperator(callId, operatorId);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{operatorId}")]
        public IActionResult UnassignOperator(int callId, int operatorId)
        {
            try
            {
                _callService.UnassignOperator(callId, operatorId);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
