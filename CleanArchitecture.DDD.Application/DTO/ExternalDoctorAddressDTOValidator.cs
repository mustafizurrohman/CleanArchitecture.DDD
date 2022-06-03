using CleanArchitecture.DDD.Core.ExtensionMethods;
using FluentValidation;

namespace CleanArchitecture.DDD.Application.DTO;

public class ExternalDoctorAddressDTOValidator : AbstractValidator<ExternalDoctorAddressDTO>
{
    public ExternalDoctorAddressDTOValidator()
    {
        SetValidationRules();
    }

    // We are only validating a subset of properties as an example
    private void SetValidationRules()
    {
        RuleFor(prop => prop.EDCMExternalID)
            .NotEqual(Guid.Empty);

        RuleFor(prop => prop.Firstname)
            .MustBeValidName();

        RuleFor(prop => prop.Lastname)
            .MustBeValidName();

        // Addresses may be validated asynchronously using a third party service
    }
}