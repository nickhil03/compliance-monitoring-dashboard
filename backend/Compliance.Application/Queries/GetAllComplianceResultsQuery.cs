using MediatR;
using Compliance.Domain.Entities;

namespace Compliance.Application.Queries
{
    public record GetAllComplianceResultsQuery : IRequest<List<ComplianceResult>>;
}