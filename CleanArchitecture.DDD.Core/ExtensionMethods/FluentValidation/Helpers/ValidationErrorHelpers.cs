﻿using CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Models;
using FluentValidation.Results;

namespace CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Helpers;

public static class ValidationErrorHelpers
{
    public static IEnumerable<ValidationErrorByProperty> GetValidationErrorByProperties(this IEnumerable<ValidationFailure> failures, bool showValue = true)
    {
        return failures
            .GroupBy(err => new PropertyNameAttemptedValue(err.PropertyName, err.AttemptedValue))
            .Select(ve => new ValidationErrorByProperty(ve, showValue));
    }

}
