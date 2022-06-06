namespace CleanArchitecture.DDD.Application.DTO.Internal;

internal class ErrorReport
{
    public ExternalDoctorAddressDTO Doctor { get; init; }
    public bool Valid { get; init; }


}

internal class ValidationErrorByProperty
{
    public string PropertyName { get; init; }
    public string ErrorMessage { get; init; }
}