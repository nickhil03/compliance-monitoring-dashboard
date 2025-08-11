using Compliance.Domain.Model;

namespace Compliance.Domain.Repositories.RefreshTokenRepos
{
    public interface IRefreshTokenRepository
    {
        void RefreshToken(string refreshToken);
        Task CreateRefreshTokenAsync(RefreshToken refreshToken);
        Task SaveRefreshTokenAsync(RefreshToken refreshToken);
    }
}
