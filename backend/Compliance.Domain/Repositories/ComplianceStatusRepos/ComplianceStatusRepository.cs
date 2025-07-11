using Compliance.Domain.Entities;
using MongoDB.Driver;

namespace Compliance.Domain.Repositories.ComplianceStatusRepos
{
    public class ComplianceStatusRepository(IMongoDatabase mongoDatabase) : IComplianceStatusRepository
    {
        private readonly IMongoCollection<ComplianceStatus> _collection = mongoDatabase.GetCollection<ComplianceStatus>("ComplianceStatus");
        public Task AddAsync(ComplianceStatus status)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ComplianceStatus>?> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public Task<ComplianceStatus?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ComplianceStatus status)
        {
            throw new NotImplementedException();
        }
    }
}
