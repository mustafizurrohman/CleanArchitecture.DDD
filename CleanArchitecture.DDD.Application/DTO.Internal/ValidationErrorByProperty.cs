namespace CleanArchitecture.DDD.Application.DTO.Internal;

internal class ValidationErrorByProperty
{
    public string PropertyName { get; init; }
    public IEnumerable<string> ErrorMessage { get; init; }
}