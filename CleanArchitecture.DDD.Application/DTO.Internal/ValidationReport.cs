namespace CleanArchitecture.DDD.Application.DTO.Internal;

public class ValidationReport<T>
    where T : class, new()
{
    public IEnumerable<ModelValidationReport<T>> ValidModelsReport { [UsedImplicitly] get; init; }
    public IEnumerable<ModelValidationReport<T>> InvalidModelsReport { [UsedImplicitly] get; init; }
}