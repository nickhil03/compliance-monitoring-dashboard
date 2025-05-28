using Compliance.Application.Commands.Create.Command;
using Compliance.Domain.Entities;
using Compliance.Domain.Repositories.ComplianceRule;
using MediatR;

namespace Compliance.Application.Commands.Create.Handler;

public class CreateComplianceResultHandler(IComplianceRuleRepository repository) : IRequestHandler<CreateComplianceResultCommand, ComplianceResult>
{
    public async Task<ComplianceResult> Handle(CreateComplianceResultCommand request, CancellationToken cancellationToken)
    {
        var newResult = new ComplianceResult
        {
            Id = Guid.NewGuid().ToString(),
            RuleName = request.RuleName,
            IsCompliant = request.IsCompliant
        };

        await repository.AddAsync(newResult);
        return newResult;
    }
}