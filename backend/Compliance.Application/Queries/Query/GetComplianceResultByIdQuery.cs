using Compliance.Domain.Entities;
using MediatR;

namespace Compliance.Application.Queries.Query;

public record GetComplianceResultByIdQuery(string Id) : IRequest<ComplianceResult>;