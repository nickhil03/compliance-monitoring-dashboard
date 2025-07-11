using Compliance.Application.Queries.Query.ComplianceItemsQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Compliance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplianceItemsController(IMediator mediator) : ControllerBase
    {
        [HttpGet("getByRuleId")]
        public async Task<IActionResult> GetByRuleId(string ruleId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ruleId))
                {
                    return BadRequest("Rule name is required.");
                }

                var complianceItems = await mediator.Send(new GetAllComplianceItemsQuery(ruleId));
                if (complianceItems == null || complianceItems.Count == 0)
                {
                    return NotFound("No compliance items found for the specified rule.");
                }

                return Ok(complianceItems);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }            
        }
    }
}
