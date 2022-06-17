using JetBrains.Annotations;

namespace CleanArchitecture.DDD.Application.DTO;

// Must be public otherwise it cannot be injected automatically to the DI Registry
[UsedImplicitly]
public class FakeDoctorAddressDTOValidator : AbstractValidator<FakeDoctorAddressDTO>
{
    public FakeDoctorAddressDTOValidator()
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
        // Example- https://www.address-validator.net/api.html
        // There might be free alternatives as well 
    }
}