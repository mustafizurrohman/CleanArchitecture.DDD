using FluentValidation.Results;

namespace CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Models;

public class ValidationErrorByProperty
{
    public string PropertyName { get; }
    public object? ProvidedValue { get; }
    public IEnumerable<string> ErrorMessages { get; }

    public ValidationErrorByProperty(IGrouping<PropertyNameAttemptedValue, ValidationFailure> grp, bool showValue = true)
    {
        PropertyName = grp.Key.Name;
        
        ProvidedValue = showValue
            ? grp.Select(err => err.AttemptedValue).Distinct().Single()
            : "*** (Hidden)";

        ErrorMessages = grp.Select(err => err.ErrorMessage);
    }

}
