using Compliance.Domain.Entities;
using MediatR;

namespace Compliance.Application.Queries.Query.ComplianceResultQuery;

public record GetComplianceResultByIdQuery(string Id) : IRequest<ComplianceResult>;