using Compliance.Application.Commands.Create.Command;
using FluentValidation;

namespace Compliance.Application.Validators;

public class CreateComplianceResultCommandValidator : AbstractValidator<CreateComplianceResultCommand>
{
    public CreateComplianceResultCommandValidator()
    {
        RuleFor(x => x.RuleName).NotEmpty().WithMessage("Rule name is required.");
    }
}