using MongoDB.Bson;

namespace Compliance.Domain.Model
{
    public class RefreshToken
    {
        public ObjectId _id { get; set; } // Unique identifier for the refresh token
        public required string Token { get; set; } = string.Empty; // The actual refresh token string
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRevoked { get; set; } = false; // Indicates if the token has been revoked
        public DateTime? RevokedAt { get; set; }
        public string? ReplacedByToken { get; set; }
        public string? UserName { get; set; }
        public string? UserId { get; set; }
        public string? UserAgent { get; set; }
        public bool IsUsed { get; set; } // Indicates if the token has been used
        public string? CreatedByIp { get; set; }
    }
}