using FluentValidation.Results;

namespace CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Models;

public class ValidationErrorByProperty
{
    public string PropertyName { [UsedImplicitly] get; }
    public object? ProvidedValue { [UsedImplicitly] get; }
    public IEnumerable<string> ErrorMessages { [UsedImplicitly] get; }

    public ValidationErrorByProperty(IGrouping<PropertyNameAttemptedValue, ValidationFailure> grp, bool showValue = true)
    {
        PropertyName = grp.Key.Name;
        
        ProvidedValue = showValue
            ? grp.Select(err => err.AttemptedValue).Distinct().Single()
            : "*** (Hidden)";

        ErrorMessages = grp.Select(err => err.ErrorMessage);
    }

}
