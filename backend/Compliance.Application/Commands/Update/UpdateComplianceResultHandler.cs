using Compliance.Domain.Entities;
using MediatR;

namespace Compliance.Application.Commands.Update;

public class UpdateComplianceResultHandler : IRequestHandler<UpdateComplianceResultCommand, ComplianceResult>
{
    private readonly IMongoRepository _repository;

    public UpdateComplianceResultHandler(IMongoRepository repository)
    {
        _repository = repository;
    }

    public async Task<ComplianceResult> Handle(UpdateComplianceResultCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(request.Id) ?? throw new Exception($"Compliance result with ID {request.Id} not found.");
        
        existing.RuleName = request.RuleName;
        existing.IsCompliant = request.IsCompliant;
        //existing.CheckedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(existing);
        return existing;
    }
}
