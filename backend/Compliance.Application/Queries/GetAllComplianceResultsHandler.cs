using Compliance.Domain.Entities;
using MediatR;

namespace Compliance.Application.Queries
{
    public class GetAllComplianceResultsHandler(IMongoRepository _repository) : IRequestHandler<GetAllComplianceResultsQuery, List<ComplianceResult>>
    {
        public async Task<List<ComplianceResult>> Handle(GetAllComplianceResultsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}
