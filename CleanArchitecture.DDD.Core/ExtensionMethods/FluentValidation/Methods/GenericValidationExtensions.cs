using CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Models;

namespace CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Methods;

public static class GenericValidationExtensions
{
    public static ModelValidationReport<T> GetModelValidationReport<T>(this T model, IValidator<T> validator)
        where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(validator);
        
        var validationResult = validator.Validate(model);

        return new ModelValidationReport<T>(model, validationResult);
    }

    public static ModelValidationReport<T> GetModelValidationReport<T>(this T model)
        where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(model);

        var validator = model.GetValidator();
        return GetModelValidationReport(model, validator);
    }

    public static async Task<ModelValidationReport<T>> GetModelValidationReportAsync<T>(this T model, IValidator<T> validator)
        where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(validator);

        var validationResult = await validator.ValidateAsync(model);

        return new ModelValidationReport<T>(model, validationResult);
    }

    public static async Task<ModelValidationReport<T>> GetModelValidationReportAsync<T>(this T model)
        where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(model);

        var validator = model.GetValidator();
        return await GetModelValidationReportAsync(model, validator);
    }

}
