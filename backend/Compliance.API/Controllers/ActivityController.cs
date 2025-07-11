using Compliance.Application.Queries.Query.ActivityQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Compliance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController(IMediator mediator) : ControllerBase
    {
        [HttpGet("getRecentActivities")]
        public async Task<IActionResult> GetRecentActivities()
        {
            var activities = await mediator.Send(new GetAllRecentActivitiesQuery());

            if (activities == null || activities.Count == 0)
            {
                return NotFound("No recent activities found.");
            }

            return Ok(activities);
        }
    }
}
