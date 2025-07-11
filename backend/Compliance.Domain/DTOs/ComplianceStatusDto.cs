using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Compliance.Domain.DTOs
{
    public class ComplianceStatusDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }
        public string? Title { get; set; }
        public string? Value { get; set; }
        public string? Status { get; set; }
        public string? Color { get; set; }
    }
}
