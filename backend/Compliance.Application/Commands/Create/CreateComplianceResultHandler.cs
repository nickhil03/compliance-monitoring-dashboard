using Compliance.Domain.Entities;
using MediatR;

namespace Compliance.Application.Commands.Create;

public class CreateComplianceResultHandler(IMongoRepository repository) : IRequestHandler<CreateComplianceResultCommand, ComplianceResult>
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