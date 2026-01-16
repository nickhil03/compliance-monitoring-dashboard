using Compliance.Domain.Entities;
using MediatR;

namespace Compliance.Application.Commands.Generate.Command;

public record GenerateComplianceReportCommand(string ReportId) : IRequest<GenerateReport>;