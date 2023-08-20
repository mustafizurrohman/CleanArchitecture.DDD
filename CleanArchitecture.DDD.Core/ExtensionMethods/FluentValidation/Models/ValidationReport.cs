namespace CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Models;

public class ValidationReport<T>
    where T : class, new()
{
    public IEnumerable<ModelValidationReport<T>> ValidModelsReport { get; }
    public IEnumerable<ModelValidationReport<T>> InvalidModelsReport { get; }

    public ValidationReport(IEnumerable<ModelValidationReport<T>> validModelsReport, IEnumerable<ModelValidationReport<T>> invalidModelsReport)
    {
        ValidModelsReport = validModelsReport;
        InvalidModelsReport = invalidModelsReport;
    }
}