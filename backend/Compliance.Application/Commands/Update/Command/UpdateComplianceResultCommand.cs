using Compliance.Domain.Entities;
using MediatR;

namespace Compliance.Application.Commands.Update.Command;

public record UpdateComplianceResultCommand(string Id, string RuleName, bool IsCompliant) : IRequest<ComplianceResult>;