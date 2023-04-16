using CleanArchitecture.DDD.Core.ExtensionMethods.FluentValidation.Models;

namespace CleanArchitecture.DDD.Application.Services;

public class NotificationService : INotificationService
{
    public async Task NotifyAdminAboutInvalidData<T>(ModelCollectionValidationReport<T> modelCollectionValidationReport) 
        where T : class, new()
    {
        if (modelCollectionValidationReport.HasAllValidModels)
            return;

        // TODO: Save as HTML and send as attachment using Weischer Global Email service 
        var validationResult = modelCollectionValidationReport.ValidationReport.InvalidModelsReport.ToFormattedJsonFailSafe();
        Log.Warning(validationResult);

        LoggingHelper.LogWithSpace(() => Log.Warning("Got {countOfInvalidModels} invalid data from CRM / external system.", modelCollectionValidationReport.InvalidModels.Count()));
        LoggingHelper.LogWithSpace(() => Log.Information("In prod the admin must be informed or properly logged to gain attention ..."));

        await Task.Delay(0);
    }

}