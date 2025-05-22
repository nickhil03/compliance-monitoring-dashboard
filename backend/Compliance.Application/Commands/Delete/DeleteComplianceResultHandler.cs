using MediatR;

namespace Compliance.Application.Commands.Delete;

public class DeleteComplianceResultHandler(IMongoRepository repository) : IRequestHandler<DeleteComplianceResultCommand>
{
    public async Task Handle(DeleteComplianceResultCommand request, CancellationToken cancellationToken)
    {
        _ = await repository.GetByIdAsync(request.Id) ?? throw new Exception($"Compliance result with ID {request.Id} not found.");

        await repository.DeleteAsync(request.Id);
    }
}