namespace CleanArchitecture.DDD.Application.DTO.Internal;

public class ModelCollectionValidationReport<T> 
    where T : class, new()
{
    protected List<ModelValidationReport<T>> ValidationReportInternal { private get; init; }
    
    public ValidationReport<T> ValidationReport { get; init; } 

    public IEnumerable<T> ValidModels =>
        ValidationReportInternal.Where(model => model.Valid).Select(model => model.Model);

    public IEnumerable<T> InvalidModels =>
        ValidationReportInternal.Where(model => !model.Valid).Select(model => model.Model);

    public bool HasValidModels => ValidationReportInternal.Any(model => model.Valid);
    public int NumberOfValidModels => ValidationReportInternal.Count(model => model.Valid);

    public bool HasInvalidModels => ValidationReportInternal.Any(model => !model.Valid);
    public int NumberOfInvalidModels => ValidationReportInternal.Count(model => !model.Valid);

    public bool HasAllValidModels => ValidationReportInternal.All(model => model.Valid);
    public bool HasAllInvalidModels => ValidationReportInternal.All(model => !model.Valid);
    public int TotalNumberOfModels => ValidationReportInternal.Count();

    public ModelCollectionValidationReport(IEnumerable<ModelValidationReport<T>> validationReport)
    {
        ValidationReportInternal = Guard.Against.NullOrEmpty(validationReport, nameof(validationReport)).ToList();
        
        ValidationReport = new ValidationReport<T>()
        {
            ValidModelsReport = ValidationReportInternal.Where(vr => vr.Valid),
            InvalidModelsReport = ValidationReportInternal.Where(vr => !vr.Valid)
        };
    }

}