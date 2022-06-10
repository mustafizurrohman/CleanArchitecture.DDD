namespace CleanArchitecture.DDD.Application.ExtensionMethods;

public static class EnumerableExtensions
{
    public static ModelCollectionValidationReport<T> GetModelValidationReport<T>(this IEnumerable<T> models, IValidator<T> validator)
        where T : class, new()
    {
        models = Guard.Against.Null(models, nameof(models));
        validator = Guard.Against.Null(validator, nameof(validator));

        List<ModelValidationReport<T>> errorReport = models
            .Select(model => model.GetModelValidationReport(validator))
            .ToList();
        
        return new ModelCollectionValidationReport<T>(errorReport);
    }

}