using Compliance.Domain.Entities;

namespace Compliance.Domain.Repositories.ComplianceRule
{
    public interface IComplianceRuleRepository
    {
        Task<List<ComplianceResult>> GetAllAsync();
        Task<ComplianceResult?> GetByIdAsync(string id);
        Task AddAsync(ComplianceResult entity);
        Task UpdateAsync(ComplianceResult result);
        Task DeleteAsync(string id);
    }
}