using Compliance.Domain.Entities;
using MediatR;

namespace Compliance.Application.Commands.Create;

public record CreateComplianceResultCommand(string RuleName, bool IsCompliant) : IRequest<ComplianceResult>;