using FluentValidation.Results;

namespace CleanArchitecture.DDD.Application.DTO.Internal;

public class ValidationErrorByProperty
{
    public string PropertyName { [UsedImplicitly] get; init; }
    public object? ProvidedValue { [UsedImplicitly] get; init; }
    public IEnumerable<string> ErrorMessages { [UsedImplicitly] get; init; }

    public ValidationErrorByProperty(IGrouping<PropertyNameAttemptedValue, ValidationFailure> grp, bool showValue = true)
    {
        PropertyName = grp.Key.Name;
        
        ProvidedValue = showValue
            ? grp.Select(err => err.AttemptedValue).Distinct().Single()
            : "*** (Hidden)";

        ErrorMessages = grp.Select(err => err.ErrorMessage);
    }

}
