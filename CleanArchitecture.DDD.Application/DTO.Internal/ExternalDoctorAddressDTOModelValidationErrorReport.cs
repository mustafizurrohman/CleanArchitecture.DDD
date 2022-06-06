namespace CleanArchitecture.DDD.Application.DTO.Internal;

internal class ExternalDoctorAddressDTOModelValidationErrorReport
{
    public ExternalDoctorAddressDTO Doctor { get; init; }
    public bool Valid { get; init; }
    public IEnumerable<ValidationErrorByProperty> ModelErrors { get; init; }
}
