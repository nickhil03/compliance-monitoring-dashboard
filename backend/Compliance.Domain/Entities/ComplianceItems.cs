using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Compliance.Domain.Entities
{
    [BsonIgnoreExtraElements]
    public class ComplianceItems
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }

        [BsonElement("Rule")]
        public required string Rule { get; set; }

        [BsonElement("Items")]
        public List<ComplianceDetail>? Items { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class ComplianceDetail
    {
        [BsonElement("Id")]
        public string? ItemId { get; set; }

        [BsonElement("Name")]
        public string? Name { get; set; }

        [BsonElement("Status")]
        public string? Status { get; set; }

        [BsonElement("Owner")]
        public string? Owner { get; set; }

        [BsonElement("DueDate")]
        public string? DueDate { get; set; }
    }
}
