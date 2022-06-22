namespace CleanArchitecture.DDD.Application.DTO.Internal;

public class ValidationReport<T>
    where T : class, new()
{
    public IEnumerable<ModelValidationReport<T>> ValidModelsReport { get; init; }
    public IEnumerable<ModelValidationReport<T>> InvalidModelsReport { get; init; }
}