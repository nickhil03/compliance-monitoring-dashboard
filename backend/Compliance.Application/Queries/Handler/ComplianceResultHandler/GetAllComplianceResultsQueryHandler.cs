using Compliance.Application.Queries.Query.ComplianceResultQuery;
using Compliance.Domain.Entities;
using Compliance.Domain.Repositories.ComplianceRule;
using MediatR;

namespace Compliance.Application.Queries.Handler.ComplianceResultHandler
{
    public class GetAllComplianceResultsQueryHandler(IComplianceRuleRepository _repository) : IRequestHandler<GetAllComplianceResultsQuery, List<ComplianceResult>>
    {
        public async Task<List<ComplianceResult>> Handle(GetAllComplianceResultsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}
