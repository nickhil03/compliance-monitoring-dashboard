using Compliance.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Compliance.Domain.Repositories.Activity
{
    public class RecentActivitiesRepository(IMongoDatabase database) : IRecentActivitiesRepository
    {
        private readonly IMongoCollection<RecentActivity> _recentActivities = database.GetCollection<RecentActivity>("RecentActivities");

        public async Task CreateRecentActivityAsync(RecentActivity recentActivity, CancellationToken cancellationToken)
        {
            await _recentActivities.InsertOneAsync(recentActivity, cancellationToken: cancellationToken);
        }

        public async Task DeleteRecentActivityAsync(string id, CancellationToken cancellationToken)
        {
            await _recentActivities.DeleteOneAsync(
                activity => activity.Id == id, cancellationToken: cancellationToken);
        }

        public async Task<List<RecentActivity>> GetAllRecentActivitiesAsync(CancellationToken cancellationToken)
        {
            return await _recentActivities.Find(_ => true)
                .SortByDescending(activity => activity.Date)
                .Limit(4)
                .ToListAsync(cancellationToken);
        }

        public async Task<RecentActivity?> GetRecentActivityByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await _recentActivities.Find(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task UpdateRecentActivityAsync(RecentActivity recentActivity, CancellationToken cancellationToken)
        {
            await _recentActivities.ReplaceOneAsync(
                activity => activity.Id == recentActivity.Id,
                recentActivity,
                cancellationToken: cancellationToken);
        }
    }
}
