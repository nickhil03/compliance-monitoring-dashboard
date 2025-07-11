using Compliance.Domain.Entities;
using MediatR;

namespace Compliance.Application.Queries.Query.ActivityHandler;
public record GetAllRecentActivitiesQuery : IRequest<List<RecentActivity>>;
