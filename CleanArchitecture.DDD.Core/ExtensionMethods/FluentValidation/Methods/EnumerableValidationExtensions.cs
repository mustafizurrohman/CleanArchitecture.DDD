using System.Diagnostics;
using System.Reflection;
using CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Exceptions;
using CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Models;
// ReSharper disable MemberCanBePrivate.Global

namespace CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Methods;

public static class EnumerableValidationExtensions
{
    public static ModelCollectionValidationReport<T> GetModelValidationReport<T>(this IEnumerable<T> models, IValidator<T> validator)
        where T : class, new()
    {
        models = Guard.Against.Null(models);
        validator = Guard.Against.Null(validator);

        var errorReport = models
            .Select(model => model.GetModelValidationReport(validator));
        
        return new ModelCollectionValidationReport<T>(errorReport);
    }

    // TODO: Test this throughly!
    // If this does not work then use the version from above after injecting the validator using DI
    public static ModelCollectionValidationReport<T> GetModelValidationReport<T>(this IEnumerable<T> models)
        where T : class, new()
    {
        models = Guard.Against.NullOrEmpty(models);
        models = models.ToList();

        var validator = models.First().GetValidator();
        return GetModelValidationReport(models, validator);
    }

    public static Task<ModelCollectionValidationReport<T>> GetModelValidationReportAsync<T>(this IEnumerable<T> models, IValidator<T> validator)
        where T : class, new()
    {
        models = Guard.Against.NullOrEmpty(models);
        validator = Guard.Against.Null(validator);

        var errorReport = models
            .Select(async model => await model.GetModelValidationReportAsync(validator))
            .Select(modelValidationResult => modelValidationResult.Result);

        return Task.FromResult(new ModelCollectionValidationReport<T>(errorReport));
    }

    // TODO: Test this throughly!
    // TestUseCase: Test when the validator uses DI
    // If this does not work then use the version from above after injecting the validator using DI
    public static async Task<ModelCollectionValidationReport<T>> GetModelValidationReportAsync<T>(this IEnumerable<T> models)
        where T : class, new()
    {
        models = Guard.Against.NullOrEmpty(models);
        models = models.ToList();

        var validatorInstance = models.First().GetValidator();
        return await GetModelValidationReportAsync(models, validatorInstance);
    }

    public static IValidator<T> GetValidator<T>(this T model)
    {
        IValidator<T> validatorInstance;

        try
        {
            var validatorType = typeof(AbstractValidator<>);
            var genericType = validatorType.MakeGenericType(typeof(T));

            var validatorTypeInstance = Assembly.GetAssembly(typeof(T))
                ?.GetTypes()
                .FirstOrDefault(typ => typ.IsSubclassOf(genericType))
                ?? throw new ValidatorNotDefinedException(typeof(T));

            validatorInstance = (IValidator<T>) Activator.CreateInstance(validatorTypeInstance)!;

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