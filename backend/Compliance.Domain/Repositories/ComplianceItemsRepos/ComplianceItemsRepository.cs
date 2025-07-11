using Compliance.Domain.Entities;
using MongoDB.Driver;

namespace Compliance.Domain.Repositories.ComplianceItemsRepos
{
    public class ComplianceItemsRepository(IMongoDatabase mongoDatabase) : IComplianceItemsRepository
    {
        private readonly IMongoCollection<ComplianceRuleItems> _complianceItemsCollection = mongoDatabase.GetCollection<ComplianceRuleItems>("ComplianceItems");
        public Task AddComplianceItemAsync(ComplianceRuleItems complianceItem)
        {
            throw new NotImplementedException();
        }

        public Task DeleteComplianceItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ComplianceRuleItems>> GetAllComplianceItemsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ComplianceRuleItems?> GetComplianceItemByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ComplianceItems>?> GetComplianceItemsByRuleAsync(string ruleId)
        {
            var filter = Builders<ComplianceRuleItems>.Filter.Eq(item => item.Rule, ruleId);
            var complianceRuleItem = await _complianceItemsCollection.Find(filter).FirstOrDefaultAsync();
            if (complianceRuleItem == null)
            {
                return null; // No compliance items found for the given ruleId
            }

            return complianceRuleItem.Items;
        }

        public Task UpdateComplianceItemAsync(ComplianceRuleItems complianceItem)
        {
            throw new NotImplementedException();
        }
    }
}
