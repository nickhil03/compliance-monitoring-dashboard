using Compliance.Application.Commands.Create.Command;
using Compliance.Application.Commands.Delete.Command;
using Compliance.Application.Commands.Update.Command;
using Compliance.Application.Queries.Query.ComplianceResultQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Compliance.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ComplianceController(IMediator _mediator) : ControllerBase
    {
        [HttpGet("get-all")]
        public async Task<IActionResult> GetCompliance()
        {
            var result = await _mediator.Send(new GetAllComplianceResultsQuery());
            if (result == null || result.Count == 0)
            {
                return NotFound("No compliance results found.");
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpecificCompliance(string id)
        {
            var result = await _mediator.Send(new GetComplianceResultByIdQuery(id));
            return Ok(result);
        }

        [HttpPost("add-compliance")]
        public async Task<IActionResult> Create([FromBody] CreateComplianceResultCommand command)
        {
            var created = await _mediator.Send(command);
            return CreatedAtAction(nameof(Create), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateComplianceResultCommand command)
        {
            if (id != command.Id) return BadRequest("ID mismatch.");
            var updated = await _mediator.Send(command);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(new DeleteComplianceResultCommand(id));
            return NoContent();
        }

        [HttpGet("compliance-pdf")]
        public IActionResult GetComplianceReport()
        {
            // Simulate generating a PDF report
            byte[] pdfBytes = System.IO.File.ReadAllBytes("path/to/compliance-report.pdf");
            return File(pdfBytes, "application/pdf", "ComplianceReport.pdf");
        }
    }
}
