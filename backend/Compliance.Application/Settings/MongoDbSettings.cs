namespace Compliance.Application.Settings
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string ComplianceResultsCollection { get; set; } = "ComplianceResults";
    }
}
