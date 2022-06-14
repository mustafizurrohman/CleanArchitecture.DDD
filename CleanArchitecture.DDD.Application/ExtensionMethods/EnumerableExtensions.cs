using System.Reflection;
using CleanArchitecture.DDD.Application.Exceptions;

namespace CleanArchitecture.DDD.Application.ExtensionMethods;

public static class EnumerableExtensions
{
    public static ModelCollectionValidationReport<T> GetModelValidationReport<T>(this IEnumerable<T> models, IValidator<T> validator)
        where T : class, new()
    {
        models = Guard.Against.Null(models, nameof(models));
        validator = Guard.Against.Null(validator, nameof(validator));

        var errorReport = models
            .Select(model => model.GetModelValidationReport(validator));
        
        return new ModelCollectionValidationReport<T>(errorReport);
    }

    // TODO: Test this throughly!
    // If this does not work then use the version from above after injecting the validator using DI
    public static ModelCollectionValidationReport<T> GetModelValidationReport<T>(this IEnumerable<T> models)
        where T : class, new()
    {
        models = Guard.Against.NullOrEmpty(models, nameof(models));
        models = models.ToList();

        var validator = models.First().GetValidator();
        return GetModelValidationReport(models, validator);
    }

    public static Task<ModelCollectionValidationReport<T>> GetModelValidationReportAsync<T>(this IEnumerable<T> models, IValidator<T> validator)
        where T : class, new()
    {
        models = Guard.Against.NullOrEmpty(models, nameof(models));
        validator = Guard.Against.Null(validator, nameof(validator));

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
        models = Guard.Against.NullOrEmpty(models, nameof(models));
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
            var evt = validatorType.MakeGenericType(typeof(T));

            var validatorTypeInstance = Assembly.GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(typ => typ.IsSubclassOf(evt));

            if (validatorTypeInstance is null)
                throw new ValidatorNotFoundException(typeof(T));

            validatorInstance = (IValidator<T>) Activator.CreateInstance(validatorTypeInstance)!;

        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);

            if (ex is ValidatorNotFoundException)
                throw;

            throw new ValidatorInitializationException(typeof(T));
        }

        return validatorInstance;
    }


}