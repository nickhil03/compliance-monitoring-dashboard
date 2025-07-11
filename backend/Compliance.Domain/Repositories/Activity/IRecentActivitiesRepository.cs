using Compliance.Domain.Entities;
using MongoDB.Bson;

namespace Compliance.Domain.Repositories.Activity
{
    public interface IRecentActivitiesRepository
    {
        Task<List<RecentActivity>> GetAllRecentActivitiesAsync(CancellationToken cancellationToken);
        Task<RecentActivity?> GetRecentActivityByIdAsync(string id, CancellationToken cancellationToken);
        Task CreateRecentActivityAsync(RecentActivity recentActivity, CancellationToken cancellationToken);
        Task UpdateRecentActivityAsync(RecentActivity recentActivity, CancellationToken cancellationToken);
        Task DeleteRecentActivityAsync(string id, CancellationToken cancellationToken);
    }
}
