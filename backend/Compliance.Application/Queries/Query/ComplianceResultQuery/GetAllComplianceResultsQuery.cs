using MediatR;
using Compliance.Domain.Entities;

namespace Compliance.Application.Queries.Query.ComplianceResultQuery
{
    public record GetAllComplianceResultsQuery : IRequest<List<ComplianceResult>>;
}