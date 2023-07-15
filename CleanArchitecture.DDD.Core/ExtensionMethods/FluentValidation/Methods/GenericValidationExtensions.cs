using System.Collections;
using System.Diagnostics;
using System.Reflection;
using CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Exceptions;
using CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Models;

namespace CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Methods;

public static class GenericValidationExtensions
{
    public static ModelValidationReport<T> GetModelValidationReport<T>(this T model, IValidator<T> validator, bool showModelValue = true)
        where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(validator);
        
        var validationResult = validator.Validate(model);

        return new ModelValidationReport<T>(model, validationResult, showModelValue);
    }

    public static ModelValidationReport<T> GetModelValidationReport<T>(this T model, bool showModelValue = true)
        where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(model);

        var validator = model.GetValidator();
        return GetModelValidationReport(model, validator, showModelValue);
    }

    public static async Task<ModelValidationReport<T>> GetModelValidationReportAsync<T>(this T model, IValidator<T> validator, bool showModelValue = true, CancellationToken cancellationToken = default)
        where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(validator);

        var validationResult = await validator.ValidateAsync(model, cancellationToken);

        return new ModelValidationReport<T>(model, validationResult, showModelValue);
    }

    public static async Task<ModelValidationReport<T>> GetModelValidationReportAsync<T>(this T model, bool showModelValue = true, CancellationToken cancellationToken = default)
        where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(model);

        var validator = model.GetValidator();
        return await GetModelValidationReportAsync(model, validator, showModelValue, cancellationToken);
    }

    private static IValidator<T> GetValidator<T>(this T _)
        where T : class, new()
    {
        IValidator<T> validatorInstance;

        try
        {
            var validatorType = typeof(AbstractValidator<>);
            var genericType = validatorType.MakeGenericType(typeof(T));

            var validatorTypeInstance = Assembly.GetAssembly(typeof(T))
                                            ?.GetTypes()
                                            .First(typ => typ.IsSubclassOf(genericType))
                                        ?? throw new ValidatorNotDefinedException(typeof(T));

            validatorInstance = (IValidator<T>)Activator.CreateInstance(validatorTypeInstance)!;

        }
        catch (Exception ex)
        {
            Log.Error(ex.Demystify(), ex.Message);

            if (ex is ValidatorNotDefinedException)
                throw;
            
            throw new ValidatorInitializationException(typeof(T), ex);
        }

        return validatorInstance;
    }

}
