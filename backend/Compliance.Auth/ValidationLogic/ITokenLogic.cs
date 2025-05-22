using System.Security.Claims;

namespace Compliance.Auth.ValidationLogic
{
    public interface ITokenLogic
    {
        string GenerateJwtToken(string username);
        IEnumerable<Claim> ReadJwtTokenClaims(string token);
        ClaimsPrincipal ValidateJwtToken(string token);
    }
}
