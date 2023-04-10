using FluentValidation.Results;

namespace CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Models;

public static class ValidationErrorHelpers
{
    public static IEnumerable<ValidationErrorByProperty> GetValidationErrorByProperties(this IEnumerable<ValidationFailure> failures, bool showValue = true)
    {
        return failures
            .GroupBy(err => new PropertyNameAttemptedValue(err.PropertyName, err.AttemptedValue))
            .Select(ve => new ValidationErrorByProperty(ve, showValue));
    }

}
