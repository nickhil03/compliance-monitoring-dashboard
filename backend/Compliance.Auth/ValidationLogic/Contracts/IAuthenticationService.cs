using Compliance.Domain.Model;
using System.Linq.Expressions;

namespace Compliance.Auth.ValidationLogic.Contracts
{
    public interface IAuthenticationService
    {
        Task<bool> AuthenticateUserAsync(string username, string password);
        Task<bool> RegisterUserAsync(UserRegisterModel userModel);
        Task SaveRefreshTokenAsync(RefreshToken refreshToken);
        Task UpdateRefreshTokenAsync(RefreshToken refreshToken);
        Task<List<RefreshToken>> GetRefreshTokensAsync(params Expression<Func<RefreshToken, bool>>[] predicate);
        Task<User?> GetUserByUsernameAsync(string username);
    }
}
