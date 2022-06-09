namespace CleanArchitecture.DDD.Application.DTO.Internal;

public class ValidationErrorByProperty
{
    public string PropertyName { get; init; }
    public object? AttemptedValue { get; init; }
    public IEnumerable<string> ErrorMessages { get; init; }
    
}