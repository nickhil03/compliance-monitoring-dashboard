using Compliance.Domain.Entities;
using MediatR;

namespace Compliance.Application.Queries.Query.ActivityQuery;
public record GetAllRecentActivitiesQuery : IRequest<List<RecentActivity>>;
