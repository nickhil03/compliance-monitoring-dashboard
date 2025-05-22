using MediatR;

namespace Compliance.Application.Commands.Delete;

public record DeleteComplianceResultCommand(string Id) : IRequest;