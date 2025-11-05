using Compliance.Domain.Entities;
using MongoDB.Driver;

namespace Compliance.Domain.Repositories.ComplianceItemsRepos
{
    public class ComplianceItemsRepository(IMongoDatabase mongoDatabase) : IComplianceItemsRepository
    {
        private readonly IMongoCollection<ComplianceItems> _complianceItemsCollection = mongoDatabase.GetCollection<ComplianceItems>(nameof(ComplianceItems));

        public async Task<List<ComplianceItem>?> GetComplianceItemsByRuleAsync(string ruleId)
        {
            var filter = Builders<ComplianceItems>.Filter.Eq(item => item.Rule, ruleId);
            var complianceRuleItem = await _complianceItemsCollection.Find(filter).FirstOrDefaultAsync();
            if (complianceRuleItem == null)
            {
                return null; // No compliance items found for the given ruleId
            }

            return complianceRuleItem.Items;
        }
    }
}
