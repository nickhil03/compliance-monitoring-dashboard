using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Compliance.Domain.Entities
{
    public class RecentActivity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }
        public required string Type { get; set; }
        public string? Description { get; set; }
        public DateOnly Date { get; set; }
    }
}
