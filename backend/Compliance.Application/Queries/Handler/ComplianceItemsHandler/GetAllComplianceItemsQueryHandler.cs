using Compliance.Application.Queries.Query.ComplianceItemsQuery;
using Compliance.Domain.Entities;
using Compliance.Domain.Repositories.ComplianceItemsRepos;
using MediatR;

namespace Compliance.Application.Queries.Handler.ComplianceItemsHandler
{
    public record GetAllComplianceItemsQueryHandler(IComplianceItemsRepository ComplianceItemsRepository) : IRequestHandler<GetAllComplianceItemsQuery, List<ComplianceItems>>
    {
        public async Task<List<ComplianceItems>> Handle(GetAllComplianceItemsQuery request, CancellationToken cancellationToken)
        {
            var complianceItems = await ComplianceItemsRepository.GetComplianceItemsByRuleAsync(request.RuleId);

            return complianceItems ?? throw new InvalidOperationException($"Compliance item with rule name '{request.RuleId}' was not found.");
        }
    }
}
