using Compliance.Domain.Entities;

public interface IMongoRepository
{
    Task<List<ComplianceResult>> GetAllAsync();
    Task<ComplianceResult?> GetByIdAsync(string id);
    Task AddAsync(ComplianceResult entity);
    Task UpdateAsync(ComplianceResult result);
    Task DeleteAsync(string id);
}