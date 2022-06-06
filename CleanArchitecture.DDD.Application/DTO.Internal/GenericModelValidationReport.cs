namespace CleanArchitecture.DDD.Application.DTO.Internal;

internal class GenericModelValidationReport<T>
    where T : class, new()
{
    public T Model { get; init; }
    public bool Valid { get; init; }
    public IEnumerable<ValidationErrorByProperty> ModelErrors { get; init; }
}