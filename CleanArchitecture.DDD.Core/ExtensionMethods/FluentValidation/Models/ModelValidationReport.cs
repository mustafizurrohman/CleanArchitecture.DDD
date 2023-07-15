using System.Diagnostics.CodeAnalysis;
using CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Helpers;

namespace CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Models;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class ModelValidationReport<T>
    where T : class, new()
{
    public T Model { get; }
    public bool Valid { get; }
    public IEnumerable<ValidationErrorByProperty> ModelErrors { get; }
    public int NumberOfModelErrors { get; }

    public ModelValidationReport(T model, FluentValidationResult validationResult, bool showModelValue = true)
    {
        Model = model;
        Valid = validationResult.IsValid;
        ModelErrors = validationResult.Errors.GetValidationErrorByProperties(showModelValue);
        NumberOfModelErrors = ModelErrors.Count();
    }
}