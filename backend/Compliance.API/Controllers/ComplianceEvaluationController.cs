using Compliance.Application.Commands.Evaluate.Command;
using Compliance.Infrastructure.Messaging.Publisher;
using Microsoft.AspNetCore.Mvc;

namespace Compliance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplianceEvaluationController(IRabbitMqPublisher _publisher, IConfiguration _configuration) : ControllerBase
    {
        [HttpPost("evaluate")]
        public IActionResult TriggerEvaluation([FromBody] string ruleId)
        {
            var command = new EvaluateComplianceCommand(ruleId);
            _publisher.Publish(_configuration["queueName"], command);
            return Ok("Evaluation queued.");
        }
    }
}
