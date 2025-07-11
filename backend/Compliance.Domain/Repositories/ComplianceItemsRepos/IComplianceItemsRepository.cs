using Compliance.Domain.Entities;

namespace Compliance.Domain.Repositories.ComplianceItemsRepos
{
    public interface IComplianceItemsRepository
    {
        Task<List<ComplianceRuleItems>> GetAllComplianceItemsAsync();
        Task<ComplianceRuleItems?> GetComplianceItemByIdAsync(string id);
        Task AddComplianceItemAsync(ComplianceRuleItems complianceItem);
        Task UpdateComplianceItemAsync(ComplianceRuleItems complianceItem);
        Task DeleteComplianceItemAsync(string id);
        Task<List<ComplianceItems>?> GetComplianceItemsByRuleAsync(string ruleId);
    }
}
