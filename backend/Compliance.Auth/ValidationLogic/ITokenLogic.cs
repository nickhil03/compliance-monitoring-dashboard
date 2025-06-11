using Compliance.Domain.Model;
using System.Security.Claims;

namespace Compliance.Auth.ValidationLogic
{
    public interface ITokenLogic
    {
        string GenerateJwtToken(User user);
        IEnumerable<Claim> ReadJwtTokenClaims(string token);
        ClaimsPrincipal ValidateJwtToken(string token);
    }
}
