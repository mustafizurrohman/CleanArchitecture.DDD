namespace CleanArchitecture.DDD.Application.DTO.Internal;

public class ModelValidationReport<T>
    where T : class, new()
{
    public T Model { get; init; }
    public bool Valid { get; init; }
    public IEnumerable<ValidationErrorByProperty> ModelErrors { get; init; }

    public ModelValidationReport(T model, FluentValidationResult validationResult)
    {
        Model = model;
        Valid = validationResult.IsValid;
        ModelErrors = validationResult.Errors.GetValidationErrorByProperties(true);
    }
}