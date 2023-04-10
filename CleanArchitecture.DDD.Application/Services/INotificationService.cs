using CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Models;

namespace CleanArchitecture.DDD.Application.Services;

public interface INotificationService
{
    Task NotifyAdminAboutInvalidData<T>(ModelCollectionValidationReport<T> modelCollectionValidationReport)
        where T : class, new();
}