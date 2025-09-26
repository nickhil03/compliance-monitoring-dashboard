using Compliance.Domain.Model;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Compliance.Domain.Repositories.RefreshTokenRepos
{
    public class RefreshTokenRepository(IMongoDatabase database)
        : IRefreshTokenRepository
    {
        private readonly IMongoCollection<RefreshToken> _collection = database.GetCollection<RefreshToken>(nameof(RefreshToken));

        public async Task SaveRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _collection.InsertOneAsync(refreshToken);
        }

        public async Task UpdateRefreshTokenAsync(RefreshToken refreshToken)
        {
            var filter = Builders<RefreshToken>.Filter.Eq(r => r.Id, refreshToken.Id);
            await _collection.ReplaceOneAsync(filter, refreshToken);
        }

        public async Task<List<RefreshToken>> GetNonRevokedRefreshTokensAsync(params Expression<Func<RefreshToken, bool>>[] predicates)
        {
            var filterBuilder = Builders<RefreshToken>.Filter;
            var filter = filterBuilder.Eq(r => r.IsRevoked, false);
            if (predicates != null && predicates.Length != 0)
            {
                foreach (var predicate in predicates)
                {
                    filter &= filterBuilder.Where(predicate);
                }
            }

            return await _collection.Find(filter).ToListAsync();
        }
    }
}
