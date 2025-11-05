using Compliance.Domain.Entities;
using MediatR;

namespace Compliance.Application.Queries.Query.ComplianceItemsQuery;
public record GetAllComplianceItemsQuery(string RuleId) : IRequest<List<ComplianceItem>>;
