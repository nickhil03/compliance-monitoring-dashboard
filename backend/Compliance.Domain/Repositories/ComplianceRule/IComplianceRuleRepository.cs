using Compliance.Domain.Entities;

namespace Compliance.Domain.Repositories.ComplianceRule
{
    public interface IComplianceRuleRepository : IMongoRepository<ComplianceResult>{}
}