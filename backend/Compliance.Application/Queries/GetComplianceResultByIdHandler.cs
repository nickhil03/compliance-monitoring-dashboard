using Compliance.Domain.Entities;
using MediatR;

namespace Compliance.Application.Queries;

public class GetComplianceResultByIdHandler(IMongoRepository repository) : IRequestHandler<GetComplianceResultByIdQuery, ComplianceResult>
{
    public async Task<ComplianceResult> Handle(GetComplianceResultByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetByIdAsync(request.Id);
        return result is null ? throw new Exception($"Compliance result with ID {request.Id} not found.") : result;
    }
}