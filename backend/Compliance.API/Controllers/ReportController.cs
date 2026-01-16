using Compliance.Application.Commands.Generate.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Compliance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController(IMediator _mediator) : ControllerBase
    {
        /// <summary>
        /// Initiates the generation of a compliance report.
        /// </summary>
        [HttpPost("generate")]
        public IActionResult GenerateReport([FromBody] GenerateComplianceReportCommand command)
        {
            _mediator.Send(command);
            return Accepted(new { command.ReportId, Message = "Report Generation initiated."});
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
