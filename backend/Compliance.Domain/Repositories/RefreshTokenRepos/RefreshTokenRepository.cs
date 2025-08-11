using Compliance.Domain.Model;
using MongoDB.Driver;

namespace Compliance.Domain.Repositories.RefreshTokenRepos
{
    public class RefreshTokenRepository(IMongoDatabase database)
        : IRefreshTokenRepository
    {
        private readonly IMongoCollection<RefreshToken> _collection = database.GetCollection<RefreshToken>(nameof(RefreshToken));

        public Task CreateRefreshTokenAsync(RefreshToken refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task CreateRefreshTokenAsync(RefreshToken refreshToken, string hashedToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRefreshTokenAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RefreshToken>> GetAllRefreshTokensAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RefreshToken?> GetRefreshTokenByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<RefreshToken?> GetRefreshTokenByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsValidRefreshTokenAsync(string token)
        {
            throw new NotImplementedException();
        }

        public void RefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task RevokeRefreshTokenAsync(string token)
        {
            throw new NotImplementedException();
        }

        public async Task SaveRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _collection.InsertOneAsync(refreshToken);
        }

        public Task UpdateRefreshTokenAsync(RefreshToken refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
