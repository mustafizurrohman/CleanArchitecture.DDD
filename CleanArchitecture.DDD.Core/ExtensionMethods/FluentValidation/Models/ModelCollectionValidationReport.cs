using System.Diagnostics.CodeAnalysis;

namespace CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Models;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class ModelCollectionValidationReport<T> 
    where T : class, new()
{
    private IEnumerable<ModelValidationReport<T>> ValidationReportInternal { get; }

    private IEnumerable<ModelValidationReport<T>> ValidModelReports =>
        ValidationReportInternal.Where(model => model.Valid);

    private IEnumerable<ModelValidationReport<T>> InValidModelReports =>
        ValidationReportInternal.Where(model => !model.Valid);


    public ValidationReport<T> ValidationReport { get; } 

    public IEnumerable<T> ValidModels => ValidModelReports
            .Select(model => model.Model);

    public IEnumerable<T> InvalidModels => InValidModelReports
            .Select(model => model.Model);

    public bool HasValidModels => ValidationReportInternal.Any(model => model.Valid);
    public int NumberOfValidModels => ValidationReportInternal.Count(model => model.Valid);

    public bool HasInvalidModels => ValidationReportInternal.Any(model => !model.Valid);
    public int NumberOfInvalidModels => ValidationReportInternal.Count(model => !model.Valid);

    public bool HasAllValidModels => ValidationReportInternal.All(model => model.Valid);
    public bool HasAllInvalidModels => ValidationReportInternal.All(model => !model.Valid);
    public int TotalNumberOfModels => ValidationReportInternal.Count();

    public double PercentageValidModels => NumberOfValidModels.GetPercentageOf(TotalNumberOfModels);
    public double PercentageInvalidModels => Math.Round(100 - PercentageValidModels, 2);

    public ModelCollectionValidationReport(IEnumerable<ModelValidationReport<T>> validationReport)
    {
        ValidationReportInternal = Guard.Against.NullOrEmpty(validationReport).ToList();
        
        ValidationReport = new ValidationReport<T>(ValidModelReports, InValidModelReports);
    }

}