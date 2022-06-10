namespace CleanArchitecture.DDD.Application.DTO.Internal;

public class ModelCollectionValidationReport<T> 
    where T : class, new()
{
    
    public IEnumerable<ModelValidationReport<T>> Report { get; init; }
    
    public IEnumerable<T> ValidModels =>
        Report.Where(r => r.Valid).Select(r => r.Model).ToList();

    public IEnumerable<T> InvalidModels =>
        Report.Where(r => !r.Valid).Select(r => r.Model).ToList();

    public bool HasInvalidModels => Report.Any(r => !r.Valid);

    public bool HasValidModels => Report.Any(r => r.Valid);

    public bool HasAllValidModels => Report.All(r => r.Valid);

    public bool HasAllInvalidModels => Report.All(r => !r.Valid);

    public ModelCollectionValidationReport(IEnumerable<ModelValidationReport<T>> report)
    {
        Report = report;
    }

    public ModelCollectionValidationReport(ModelValidationReport<T> report)
    {
        Report = new List<ModelValidationReport<T>>()
        {
            report
        };
    }

}