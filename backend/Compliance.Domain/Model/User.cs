using MongoDB.Bson;

namespace Compliance.Domain.Model
{
    public class User
    {        
        public required ObjectId _id { get; set; }
        public required string Name { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public string? Email { get; set; }
        public string? Roles { get; set; }
    }
}