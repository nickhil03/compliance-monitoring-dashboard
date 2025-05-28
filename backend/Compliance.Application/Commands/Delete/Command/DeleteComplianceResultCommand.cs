using MediatR;

namespace Compliance.Application.Commands.Delete.Command;

public record DeleteComplianceResultCommand(string Id) : IRequest;