using MediatR;
namespace Compliance.Application.Commands.Evaluate.Command;
public record EvaluateComplianceCommand(string RuleId) : IRequest;