using Compliance.Application.Queries.Query.ComplianceStatusQuery;
using Compliance.Domain.Entities;
using Compliance.Domain.Repositories.ComplianceStatusRepos;
using MediatR;

namespace Compliance.Application.Queries.Handler.GetComplianceStatusHandler
{
    public record GetComplianceStatusQueryHandler(IComplianceStatusRepository Repository) : IRequestHandler<GetComplianceStatusQuery, List<ComplianceStatus>?>
    {
        public async Task<List<ComplianceStatus>?> Handle(GetComplianceStatusQuery request, CancellationToken cancellationToken)
        {
            return await Repository.GetAllAsync();
        }
    }
}
