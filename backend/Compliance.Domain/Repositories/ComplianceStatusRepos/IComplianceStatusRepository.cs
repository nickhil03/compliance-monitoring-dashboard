using Compliance.Domain.Entities;

namespace Compliance.Domain.Repositories.ComplianceStatusRepos
{
    public interface IComplianceStatusRepository
    {
        Task<List<ComplianceStatus>?> GetAllAsync();
        Task<ComplianceStatus?> GetByIdAsync(string id);
        Task AddAsync(ComplianceStatus status);
        Task UpdateAsync(ComplianceStatus status);
        Task DeleteAsync(string id);
    }
}
