using Compliance.Domain.Entities;

namespace Compliance.Domain.Repositories.ComplianceItemsRepos
{
    public interface IComplianceItemsRepository
    {
        Task<List<ComplianceDetail>?> GetComplianceItemsByRuleAsync(string ruleId);
    }
}
