using Compliance.Domain.Entities;

namespace Compliance.Domain.Repositories.ComplianceItemsRepos
{
    public interface IComplianceItemsRepository
    {
        Task<List<ComplianceItem>?> GetComplianceItemsByRuleAsync(string ruleId);
    }
}
