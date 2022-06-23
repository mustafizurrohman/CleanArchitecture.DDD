namespace CleanArchitecture.DDD.Application.DTO.Internal;

public class ValidationErrorByProperty
{
    public string PropertyName { [UsedImplicitly] get; init; }
    public object? ProvidedValue { [UsedImplicitly] get; init; }
    public IEnumerable<string> ErrorMessages { [UsedImplicitly] get; init; }
}
