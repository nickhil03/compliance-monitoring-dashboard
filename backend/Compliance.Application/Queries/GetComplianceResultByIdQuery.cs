using Compliance.Domain.Entities;
using MediatR;

namespace Compliance.Application.Queries;

public record GetComplianceResultByIdQuery(string Id) : IRequest<ComplianceResult>;