using MediatR;
using Compliance.Domain.Entities;

namespace Compliance.Application.Queries.Query
{
    public record GetAllComplianceResultsQuery : IRequest<List<ComplianceResult>>;
}