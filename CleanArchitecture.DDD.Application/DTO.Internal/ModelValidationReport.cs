namespace CleanArchitecture.DDD.Application.DTO.Internal;

public class ModelValidationReport<T>
    where T : class, new()
{
    public T Model { get; init; }
    public bool Valid { get; init; }
    public IEnumerable<ValidationErrorByProperty> ModelErrors { get; init; }

    public ModelValidationReport(T model, FluentValidation.Results.ValidationResult validationResult)
    {
        Model = model;
        Valid = validationResult.IsValid;
        ModelErrors = validationResult.Errors
            .GroupBy(e => new {e.PropertyName, e.AttemptedValue})
            .Select(e => new ValidationErrorByProperty
            {
                PropertyName = e.Key.PropertyName,
                ProvidedValue = e.Select(err => err.AttemptedValue).Distinct().Single(),
                ErrorMessages = e.Select(err => err.ErrorMessage).ToList()
            });
    }
}