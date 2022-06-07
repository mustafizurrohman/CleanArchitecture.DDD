namespace CleanArchitecture.DDD.Application.DTO.Internal;

internal class ModelValidationReport<T> : GenericModelValidationReport<T>
    where T : class, new()
{
    
    public IEnumerable<GenericModelValidationReport<T>> Report { get; init; }
    
    public IEnumerable<T> ValidModels =>
        Report.Where(r => r.Valid).Select(r => r.Model).ToList();

    public IEnumerable<T> InvalidModels =>
        Report.Where(r => !r.Valid).Select(r => r.Model).ToList();

    public bool HasInvalidModels => Report.Any(r => !r.Valid);

    public bool HasAllValidModels => Report.All(r => r.Valid);

    public ModelValidationReport(IEnumerable<GenericModelValidationReport<T>> report)
    {
        Report = report;
    }

    public ModelValidationReport(GenericModelValidationReport<T> report)
    {
        Report = new List<GenericModelValidationReport<T>>()
        {
            report
        };
    }

}