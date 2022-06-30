namespace CleanArchitecture.DDD.Application.Services;

public interface INotificationService
{
    Task NotifyAdminAboutInvalidData<T>(ModelCollectionValidationReport<T> modelCollectionValidationReport)
        where T : class, new();
}