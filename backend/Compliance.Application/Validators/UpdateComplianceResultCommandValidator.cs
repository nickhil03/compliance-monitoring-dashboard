using Compliance.Application.Commands.Update;
using FluentValidation;

namespace Compliance.Application.Validators;

public class UpdateComplianceResultCommandValidator : AbstractValidator<UpdateComplianceResultCommand>
{
    public UpdateComplianceResultCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.RuleName).NotEmpty().WithMessage("Rule name is required.");
    }
}