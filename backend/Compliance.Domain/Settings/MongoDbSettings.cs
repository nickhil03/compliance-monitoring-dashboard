namespace Compliance.Domain.Settings
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string Database { get; set; } = null!;
        public string CollectionName { get; set; } = null!;
    }
}
