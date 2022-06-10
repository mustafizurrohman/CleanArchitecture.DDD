﻿namespace CleanArchitecture.DDD.Application.ExtensionMethods;

public static class GenericExtensions
{
    public static ModelValidationReport<T> GetModelValidationReport<T>(this T model, IValidator<T> validator)
        where T : class, new()
    {
        model = Guard.Against.Null(model, nameof(model));
        validator = Guard.Against.Null(validator, nameof(validator));

        var validationResult = validator.Validate(model);

        return new ModelValidationReport<T>
        {
            Model = model,
            Valid = validationResult.IsValid,
            ModelErrors = validationResult.Errors
                .GroupBy(e => new { e.PropertyName, e.AttemptedValue })
                .Select(e => new ValidationErrorByProperty
                {
                    PropertyName = e.Key.PropertyName,
                    AttemptedValue = e.Select(err => err.AttemptedValue).Distinct().Single(),
                    ErrorMessages = e.Select(err => err.ErrorMessage).ToList()
                })
        };
    }
}