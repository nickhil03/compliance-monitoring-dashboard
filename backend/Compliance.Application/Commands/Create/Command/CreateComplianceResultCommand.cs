using Compliance.Domain.Entities;
using MediatR;

namespace Compliance.Application.Commands.Create.Command;

public record CreateComplianceResultCommand(string RuleName, bool IsCompliant) : IRequest<ComplianceResult>;