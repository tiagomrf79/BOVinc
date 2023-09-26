using FarmsAPI.Models;
using FluentValidation;

namespace FarmsAPI.Validations;

public class HerdValidator : AbstractValidator<Herd>
{
    public HerdValidator()
    {
        RuleFor(h => h.Name)
            .NotNull().NotEmpty().WithMessage("{PropertyName} is required (fluent).")
            .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters (fluent).");

        RuleFor(h => h.Address)
            .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters (fluent).");

        RuleFor(h => h.City)
            .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters (fluent).");

        RuleFor(h => h.Region)
            .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters (fluent).");

        RuleFor(h => h.Country)
            .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters (fluent).");
    }
}
