using MongoDB.Bson;
using System.Security.Cryptography;

namespace Compliance.Domain.Model
{
    public class RefreshToken
    {
        public ObjectId _id { get; set; } // Unique identifier for the refresh token
        public static readonly TimeSpan DefaultLifetime = TimeSpan.FromDays(30); // Default lifetime for refresh tokens
        
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string? UserName { get; set; }
        public string? UserAgent { get; set; }
        public string? IpAddress { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsRevoked { get; set; } // Indicates if the token has been revoked
        public bool IsUsed { get; set; } // Indicates if the token has been used
        public bool IsActive => IsExpired || IsRevoked || IsUsed; // Token is invalid if it is expired, revoked, or used

        public DateTime Expires { get; }
        public string UserId { get; }
    }
}