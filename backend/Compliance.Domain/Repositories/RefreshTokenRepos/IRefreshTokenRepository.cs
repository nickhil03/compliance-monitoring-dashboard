using Compliance.Domain.Model;
using System.Linq.Expressions;

namespace Compliance.Domain.Repositories.RefreshTokenRepos
{
    public interface IRefreshTokenRepository
    {
        Task<List<RefreshToken>> GetNonRevokedRefreshTokensAsync(params Expression<Func<RefreshToken, bool>>[] predicates);
        Task SaveRefreshTokenAsync(RefreshToken refreshToken);
        Task UpdateRefreshTokenAsync(RefreshToken refreshToken);
    }
}
