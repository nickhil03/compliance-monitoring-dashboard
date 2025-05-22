using Compliance.Domain.DTOs;
using FluentValidation;

namespace Compliance.Domain.Validators
{
    public class ComplianceResultDtoValidator : AbstractValidator<ComplianceResultDto>
    {
        public ComplianceResultDtoValidator()
        {
            RuleFor(x => x.RuleName)
                .NotEmpty().WithMessage("RuleName is required")
                .MaximumLength(100);
        }
    }
}
