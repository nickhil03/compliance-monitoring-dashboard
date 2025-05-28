using Compliance.Application.Queries.Query;
using Compliance.Domain.Entities;
using Compliance.Domain.Repositories.ComplianceRule;
using MediatR;

namespace Compliance.Application.Queries.Handler
{
    public class GetAllComplianceResultsHandler(IComplianceRuleRepository _repository) : IRequestHandler<GetAllComplianceResultsQuery, List<ComplianceResult>>
    {
        public async Task<List<ComplianceResult>> Handle(GetAllComplianceResultsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}
