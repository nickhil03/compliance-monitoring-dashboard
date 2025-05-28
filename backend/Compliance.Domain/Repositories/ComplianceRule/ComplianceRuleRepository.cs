using Compliance.Domain.Entities;
using MongoDB.Driver;

namespace Compliance.Domain.Repositories.ComplianceRule
{
    public class ComplianceRuleRepository(IMongoDatabase database, string collectionName) : IComplianceRuleRepository
    {
        private readonly IMongoCollection<ComplianceResult> _collection = database.GetCollection<ComplianceResult>(collectionName);

        public async Task<List<ComplianceResult>> GetAllAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<ComplianceResult?> GetByIdAsync(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task AddAsync(ComplianceResult result) =>
            await _collection.InsertOneAsync(result);

        public async Task UpdateAsync(ComplianceResult result) =>
            await _collection.ReplaceOneAsync(x => x.Id == result.Id, result);

        public async Task DeleteAsync(string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);
    }
}
