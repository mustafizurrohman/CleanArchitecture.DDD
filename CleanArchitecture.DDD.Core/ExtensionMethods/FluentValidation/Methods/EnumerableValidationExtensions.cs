using System.Diagnostics;
using System.Reflection;
using CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Exceptions;
using CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Models;
// ReSharper disable MemberCanBePrivate.Global

namespace CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Methods;

public static class EnumerableValidationExtensions
{
    public static async IAsyncEnumerable<ModelValidationReport<T>> GetModelValidationReportEnumerableAsync<T>(this IEnumerable<T> models, IValidator<T> validator)
        where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(models);
        ArgumentNullException.ThrowIfNull(validator);

        foreach (var model in models)
        {
            yield return await model.GetModelValidationReportAsync(validator);
        }
    }

    public static async IAsyncEnumerable<ModelValidationReport<T>> GetModelValidationReportEnumerableAsync<T>(this IEnumerable<T> models)
        where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(models);

        var modelsList = models.DefaultIfEmpty().ToList();

        IValidator<T> validator = modelsList.GetValidator()!;

        foreach (var model in modelsList)
        {
            yield return await model.GetModelValidationReportAsync(validator!);
        }
    }

    public static ModelCollectionValidationReport<T> GetModelValidationReport<T>(this IEnumerable<T> models, IValidator<T> validator)
        where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(models);
        ArgumentNullException.ThrowIfNull(validator);

        var errorReport = models
            .Select(model => model.GetModelValidationReport(validator));

        return new ModelCollectionValidationReport<T>(errorReport);
    }

    public static ModelCollectionValidationReport<T> GetModelValidationReport<T>(this IEnumerable<T> models)
        where T : class, new()
    {
        models = Guard.Against.NullOrEmpty(models);
        models = models.ToList();

        var validator = models.GetValidator();
        return GetModelValidationReport(models, validator);
    }

    public static async Task<ModelCollectionValidationReport<T>> GetModelValidationReportAsync<T>(this IEnumerable<T> models, IValidator<T> validator)
        where T : class, new()
    {
        models = Guard.Against.NullOrEmpty(models);
        validator = Guard.Against.Null(validator);
        
        var errorReport = await models.GetModelValidationReportEnumerableAsync(validator).ToListAsync();

        return new ModelCollectionValidationReport<T>(errorReport); 
    }

    public static async Task<ModelCollectionValidationReport<T>> GetModelValidationReportAsync<T>(this IEnumerable<T> models)
        where T : class, new()
    {
        models = Guard.Against.NullOrEmpty(models);
        models = models.ToList();

        var validatorInstance = models.GetValidator();
        return await GetModelValidationReportAsync(models, validatorInstance);
    }

    private static IValidator<T> GetValidator<T>(this IEnumerable<T> _)
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