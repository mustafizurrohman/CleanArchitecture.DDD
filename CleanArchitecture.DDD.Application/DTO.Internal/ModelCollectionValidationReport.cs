using System.Diagnostics.CodeAnalysis;

namespace CleanArchitecture.DDD.Application.DTO.Internal;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class ModelCollectionValidationReport<T> 
    where T : class, new()
{
    private IEnumerable<ModelValidationReport<T>> ValidationReportInternal { get; }
    
    public ValidationReport<T> ValidationReport { get; } 

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
        
        ValidationReport = new ValidationReport<T>
        {
            ValidModelsReport = ValidationReportInternal.Where(vr => vr.Valid),
            InvalidModelsReport = ValidationReportInternal.Where(vr => !vr.Valid)
        };
    }

}