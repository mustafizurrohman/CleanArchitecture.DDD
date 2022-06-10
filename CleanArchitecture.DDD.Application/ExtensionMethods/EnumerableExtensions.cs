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
            .Select(result => result.Result);

        return Task.FromResult(new ModelCollectionValidationReport<T>(errorReport));
    }

}