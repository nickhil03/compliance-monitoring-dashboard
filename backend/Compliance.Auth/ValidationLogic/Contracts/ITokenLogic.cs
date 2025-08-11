using Compliance.Domain.Model;
using System.Security.Claims;

namespace Compliance.Auth.ValidationLogic.Contracts
{
    public interface ITokenLogic
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken(string username, string? deviceInfo, string? ipAddress = null);
        IEnumerable<Claim> ReadJwtTokenClaims(string token);
        ClaimsPrincipal ValidateJwtToken(string token);
    }
}
