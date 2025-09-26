using Compliance.Domain.Model;
using System.Security.Claims;

namespace Compliance.Auth.ValidationLogic.Contracts
{
    public interface ITokenLogic
    {
        Task<TokenModel> GenerateTokens(string username, string? userAgent, string? IpAddress, bool isRefresh);
        ClaimsPrincipal? ValidateJwtToken(string token);
        Task RevokeRefreshTokenAsync(string token, string? replacedByToken = null);
        Task RevokeAllUserTokensAsync(string userId);
        Task<string?> GetUserNameFromRefreshTokenAsync(string token);
    }
}
