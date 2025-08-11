using Compliance.Domain.Model;

namespace Compliance.Auth.ValidationLogic.Contracts
{
    public interface IAuthenticationService
    {
        Task<User?> AuthenticateUserAsync(string username, string password);
        Task<bool> RegisterUserAsync(UserRegisterModel userModel);
        Task SaveRefreshTokenAsync(RefreshToken refreshToken);
    }
}
