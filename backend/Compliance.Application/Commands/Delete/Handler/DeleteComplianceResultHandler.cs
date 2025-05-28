using Compliance.Application.Commands.Delete.Command;
using Compliance.Domain.Repositories.ComplianceRule;
using MediatR;

namespace Compliance.Application.Commands.Delete.Handler;

public class DeleteComplianceResultHandler(IComplianceRuleRepository repository) : IRequestHandler<DeleteComplianceResultCommand>
{
    public async Task Handle(DeleteComplianceResultCommand request, CancellationToken cancellationToken)
    {
        _ = await repository.GetByIdAsync(request.Id) ?? throw new Exception($"Compliance result with ID {request.Id} not found.");

        await repository.DeleteAsync(request.Id);
    }
}