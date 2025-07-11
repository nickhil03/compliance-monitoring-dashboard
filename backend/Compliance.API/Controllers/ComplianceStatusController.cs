using Compliance.Application.Queries.Query.ComplianceStatusQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Compliance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplianceStatusController(IMediator mediator) : ControllerBase
    {
        [HttpGet("get")]
        public async Task<IActionResult> GetComplianceStatus()
        {
            try
            {
                var complianceStatus = await mediator.Send(new GetComplianceStatusQuery());
                if (complianceStatus == null)
                {
                    return NotFound("Compliance status not found.");
                }
                return Ok(complianceStatus);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
