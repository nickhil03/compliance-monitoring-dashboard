using Compliance.Application.Commands.Evaluate.Command;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Compliance.Application.Commands.Evaluate.Handler
{
    public class EvaluateComplianceCommandHandler(ILogger<EvaluateComplianceCommandHandler> logger) : IRequestHandler<EvaluateComplianceCommand>
    {
        public Task Handle(EvaluateComplianceCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Processing compliance rule {request.RuleId}");
            // Your logic here
            return Task.CompletedTask;
        }
    }
}
