using Compliance.Domain.Entities;
using MediatR;

namespace Compliance.Application.Commands.Update;

public record UpdateComplianceResultCommand(string Id, string RuleName, bool IsCompliant) : IRequest<ComplianceResult>;