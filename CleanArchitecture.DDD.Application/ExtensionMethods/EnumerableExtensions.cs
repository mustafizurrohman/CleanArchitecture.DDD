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

    public static Task<ModelCollectionValidationReport<T>> GetModelValidationReportAsync<T>(this IEnumerable<T> models, IValidator<T> validator)
        where T : class, new()
    {
        models = Guard.Against.Null(models, nameof(models));
        validator = Guard.Against.Null(validator, nameof(validator));

        var errorReport = models
            .Select(async model => await model.GetModelValidationReportAsync(validator))
            .Select(modelValidationResult => modelValidationResult.Result);

        return Task.FromResult(new ModelCollectionValidationReport<T>(errorReport));
    }

    public static async Task<ModelCollectionValidationReport<T>> GetModelValidationReportAsync<T>(this IEnumerable<T> models)
        where T : class, new()
    {
        models = Guard.Against.Null(models, nameof(models));

        var validatorType = typeof(AbstractValidator<>);
        var evt = validatorType.MakeGenericType(typeof(T));

        var validatorTypeInstance = Assembly.GetExecutingAssembly()
            .GetTypes()
            .FirstOrDefault(typ => typ.IsSubclassOf(evt));

        if (validatorTypeInstance is null)
            throw new ValidatorNotFoundException(T);

        var validatorInstance = (IValidator<T>)Activator.CreateInstance(validatorTypeInstance)!;

        return await GetModelValidationReportAsync(models, validatorInstance);

    }

    
}