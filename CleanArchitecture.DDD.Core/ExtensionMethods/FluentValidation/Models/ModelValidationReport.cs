using CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Helpers;

namespace CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Models;

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