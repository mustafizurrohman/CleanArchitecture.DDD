﻿using CleanArchitecture.DDD.Application.DTO.Internal;
using FluentValidation;

namespace CleanArchitecture.DDD.Application.ExtensionMethods;

public static class EnumerableExtensions
{
    public static ModelValidationReport<T> GetModelValidationReport<T>(this IEnumerable<T> models, IValidator<T> validator)
        where T : class, new()
    {
        models = Guard.Against.Null(models, nameof(models));
        validator = Guard.Against.Null(validator, nameof(validator));

        List<GenericModelValidationReport<T>> errorReport = models
            .Select(model =>
            {
                var validationResult = validator.Validate(model);

                return new GenericModelValidationReport<T>
                {
                    Model = model,
                    Valid = validationResult.IsValid,
                    ModelErrors = validationResult.Errors
                        .Select(e => new { e.PropertyName, e.ErrorMessage })
                        .GroupBy(e => e.PropertyName)
                        .Select(e => new ValidationErrorByProperty
                        {
                            PropertyName = e.Key,
                            ErrorMessages = e.Select(err => err.ErrorMessage).ToList()
                        })
                };
            })
            .ToList();


        return new ModelValidationReport<T>(errorReport);
    }

}