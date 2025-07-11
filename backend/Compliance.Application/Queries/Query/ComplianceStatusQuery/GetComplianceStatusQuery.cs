using Compliance.Domain.Entities;
using MediatR;

namespace Compliance.Application.Queries.Query.ComplianceStatusQuery;
public record GetComplianceStatusQuery : IRequest<List<ComplianceStatus>?>;