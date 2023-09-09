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
        VerifyThatParamsAreNotNull(models, validator);

        foreach (var model in models)
        {
            yield return await model.GetModelValidationReportAsync(validator);
        }
    }

    public static async IAsyncEnumerable<ModelValidationReport<T>> GetModelValidationReportEnumerableAsync<T>(this IEnumerable<T> models)
        where T : class, new()
    {
        VerifyThatParamsAreNotNull(models);

        models = models.ToList();

        IValidator<T> validator = models.GetValidator();

        foreach (var model in models)
        {
            yield return await model.GetModelValidationReportAsync(validator);
        }
    }

    public static async IAsyncEnumerable<ModelValidationReport<T>> GetModelValidationReportEnumerableAsync<T>(this T[] models)
        where T : class, new()
    {
        VerifyThatParamsAreNotNull(models);

        IValidator<T> validator = models.GetValidator();

        foreach (var model in models)
        {
            yield return await model.GetModelValidationReportAsync(validator);
        }
    }

    public static ModelCollectionValidationReport<T> GetModelValidationReport<T>(this IEnumerable<T> models, IValidator<T> validator)
        where T : class, new()
    {
        VerifyThatParamsAreNotNull(models, validator);

        var errorReport = models
            .Select(model => model.GetModelValidationReport(validator));

        return new ModelCollectionValidationReport<T>(errorReport);
    }

    public static ModelCollectionValidationReport<T> GetModelValidationReport<T>(this List<T> models, IValidator<T> validator)
        where T : class, new()
    {
        return models.AsEnumerable().GetModelValidationReport(validator);
    }

    public static ModelCollectionValidationReport<T> GetModelValidationReport<T>(this T[] models, IValidator<T> validator)
        where T : class, new()
    {
        return models.AsEnumerable().GetModelValidationReport(validator);
    }

    public static ModelCollectionValidationReport<T> GetModelValidationReport<T>(this IEnumerable<T> models)
        where T : class, new()
    {
        VerifyThatParamsAreNotNull(models);
        models = models.ToList();

        var validator = models.GetValidator();
        return GetModelValidationReport(models, validator);
    }

    public static ModelCollectionValidationReport<T> GetModelValidationReport<T>(this List<T> models)
        where T : class, new()
    {
        return models.AsEnumerable().GetModelValidationReport();
    }

    public static ModelCollectionValidationReport<T> GetModelValidationReport<T>(this T[] models)
        where T : class, new()
    {
        return models.AsEnumerable().GetModelValidationReport();
    }

    public static async Task<ModelCollectionValidationReport<T>> GetModelValidationReportAsync<T>(this IEnumerable<T> models, IValidator<T> validator)
        where T : class, new()
    {
        VerifyThatParamsAreNotNull(models, validator);

        var errorReport = await models.GetModelValidationReportEnumerableAsync(validator).ToListAsync();

        return new ModelCollectionValidationReport<T>(errorReport); 
    }

    public static async Task<ModelCollectionValidationReport<T>> GetModelValidationReportAsync<T>(this List<T> models, IValidator<T> validator)
        where T : class, new()
    {
        return await models.AsEnumerable().GetModelValidationReportAsync(validator);
    }

    public static async Task<ModelCollectionValidationReport<T>> GetModelValidationReportAsync<T>(this T[] models, IValidator<T> validator)
        where T : class, new()
    {
        return await models.AsEnumerable().GetModelValidationReportAsync(validator);
    }

    public static async Task<ModelCollectionValidationReport<T>> GetModelValidationReportAsync<T>(this IEnumerable<T> models)
        where T : class, new()
    {
        VerifyThatParamsAreNotNull(models);
        models = models.ToList();

        var validatorInstance = models.GetValidator();
        return await GetModelValidationReportAsync(models, validatorInstance);
    }

    public static async Task<ModelCollectionValidationReport<T>> GetModelValidationReportAsync<T>(this List<T> models)
        where T : class, new()
    {
        return await models.AsEnumerable().GetModelValidationReportAsync();
    }

    public static async Task<ModelCollectionValidationReport<T>> GetModelValidationReportAsync<T>(this T[] models)
        where T : class, new()
    {
        return await models.AsEnumerable().GetModelValidationReportAsync();
    }

    private static IValidator<T> GetValidator<T>(this IEnumerable<T> _)
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

    private static void VerifyThatParamsAreNotNull(params object[] objects)
    {
        foreach (var objectInstance in objects)
        {
            ArgumentNullException.ThrowIfNull(objectInstance);
        }
    }

}
