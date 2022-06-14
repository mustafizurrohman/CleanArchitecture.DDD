namespace CleanArchitecture.DDD.Application.DTO.Internal;

public class ModelCollectionValidationReport<T> 
    where T : class, new()
{
    
    public IEnumerable<ModelValidationReport<T>> ValidationReport { get; init; }
    
    public IEnumerable<T> ValidModels =>
        ValidationReport.Where(model => model.Valid).Select(model => model.Model);

    public IEnumerable<T> InvalidModels =>
        ValidationReport.Where(model => !model.Valid).Select(model => model.Model);

    public bool HasValidModels => ValidationReport.Any(model => model.Valid);

    public bool HasInvalidModels => ValidationReport.Any(model => !model.Valid);
    
    public bool HasAllValidModels => ValidationReport.All(model => model.Valid);

    public bool HasAllInvalidModels => ValidationReport.All(model => !model.Valid);

    public ModelCollectionValidationReport(IEnumerable<ModelValidationReport<T>> validationReport)
    {
        ValidationReport = Guard.Against.NullOrEmpty(validationReport, nameof(validationReport));
    }

}