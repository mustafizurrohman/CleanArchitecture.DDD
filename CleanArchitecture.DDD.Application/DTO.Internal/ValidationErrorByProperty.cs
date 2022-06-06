namespace CleanArchitecture.DDD.Application.DTO.Internal;

internal class ValidationErrorByProperty
{
    public string PropertyName { get; init; }
    public IEnumerable<string> ErrorMessages { get; init; }
}