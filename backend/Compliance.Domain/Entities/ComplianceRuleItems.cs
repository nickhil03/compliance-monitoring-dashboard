using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Compliance.Domain.Entities
{
    public class ComplianceRuleItems
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }
        public required string Rule { get; set; }
        public List<ComplianceItems>? Items { get; set; }
    }

    public class ComplianceItems
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
        public string? Owner { get; set; }
        public DateTime DueDate { get; set; }
    }
}
