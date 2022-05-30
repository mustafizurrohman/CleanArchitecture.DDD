using CleanArchitecture.DDD.Core.ExtensionMethods;

namespace CleanArchitecture.DDD.Domain.ValueObjects;

public class NameValidator : AbstractValidator<Name>
{
    public NameValidator()
    {
        SetValidationRules();
    }

    private void SetValidationRules()
    {
        RuleFor(prop => prop.Firstname)
            .MustBeValidName();

        When(prop => !string.IsNullOrEmpty(prop.Middlename), () =>
        {
            RuleFor(prop => prop.Middlename ?? string.Empty)
                .MustBeValidName()
                .WithName("Middlename");
        });

        RuleFor(prop => prop.Lastname)
            .MustBeValidName();
    }
}