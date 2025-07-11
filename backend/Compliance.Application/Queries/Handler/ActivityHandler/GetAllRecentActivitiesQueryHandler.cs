using Compliance.Application.Queries.Query.ActivityHandler;
using Compliance.Domain.Entities;
using Compliance.Domain.Repositories.Activity;
using MediatR;

namespace Compliance.Application.Queries.Handler.ActivityHandler
{
    public class GetAllRecentActivitiesQueryHandler(IRecentActivitiesRepository recentActivities) : IRequestHandler<GetAllRecentActivitiesQuery, List<RecentActivity>>
    {
        public async Task<List<RecentActivity>> Handle(GetAllRecentActivitiesQuery request, CancellationToken cancellationToken)
        {
            return await recentActivities.GetAllRecentActivitiesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
