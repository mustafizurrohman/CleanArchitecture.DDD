using System.Diagnostics.CodeAnalysis;

namespace CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Models;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class ModelCollectionValidationReport<T> 
    where T : class, new()
{
    private IEnumerable<ModelValidationReport<T>> ValidationReportInternal { get; }
    
    public ValidationReport<T> ValidationReport { get; } 

    public IEnumerable<T> ValidModels =>
        ValidationReportInternal.Where(model => model.Valid)
            .Select(model => model.Model);

    public IEnumerable<T> InvalidModels =>
        ValidationReportInternal.Where(model => !model.Valid)
            .Select(model => model.Model);

    public bool HasValidModels => ValidationReportInternal.Any(model => model.Valid);
    public int NumberOfValidModels => ValidationReportInternal.Count(model => model.Valid);

    public bool HasInvalidModels => ValidationReportInternal.Any(model => !model.Valid);
    public int NumberOfInvalidModels => ValidationReportInternal.Count(model => !model.Valid);

    public bool HasAllValidModels => ValidationReportInternal.All(model => model.Valid);
    public bool HasAllInvalidModels => ValidationReportInternal.All(model => !model.Valid);
    public int TotalNumberOfModels => ValidationReportInternal.Count();

    public double PercentageValidModels => Math.Round(((double)NumberOfValidModels / (double)TotalNumberOfModels) * 100, 2);

    public double PercentageInvalidModels => Math.Round((double)NumberOfInvalidModels / (double)TotalNumberOfModels * 100, 2);

    public ModelCollectionValidationReport(IEnumerable<ModelValidationReport<T>> validationReport)
    {
        ValidationReportInternal = Guard.Against.NullOrEmpty(validationReport).ToList();
        
        ValidationReport = new ValidationReport<T>(ValidationReportInternal.Where(vr => vr.Valid), ValidationReportInternal.Where(vr => !vr.Valid));
    }

}