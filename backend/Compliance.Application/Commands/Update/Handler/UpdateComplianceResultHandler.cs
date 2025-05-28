using Compliance.Application.Commands.Update.Command;
using Compliance.Domain.Entities;
using Compliance.Domain.Repositories.ComplianceRule;
using MediatR;

namespace Compliance.Application.Commands.Update.Handler;

public class UpdateComplianceResultHandler : IRequestHandler<UpdateComplianceResultCommand, ComplianceResult>
{
    private readonly IComplianceRuleRepository _repository;

    public UpdateComplianceResultHandler(IComplianceRuleRepository repository)
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
