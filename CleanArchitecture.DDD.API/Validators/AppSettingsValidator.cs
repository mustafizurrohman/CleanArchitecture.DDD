using CleanArchitecture.DDD.API.Models;
using CleanArchitecture.DDD.Core.Models;

namespace CleanArchitecture.DDD.API.Validators;

public class AppSettingsValidator : AbstractValidator<AppSettings>
{
    public AppSettingsValidator()
    {
        SetValidationRules();
    }

    private void SetValidationRules()
    {
        RuleFor(prop => prop.ConnectionStrings.DDD_Db)
            .Must(BeValidDbConnectionString)
            .WithMessage("Database is not reachable");

        RuleFor(prop => prop.HealthChecksUI.EvaluationTimeOnSeconds)
            .InclusiveBetween(1, 60);

        RuleFor(prop => prop.HealthChecksUI.MinimumSecondsBetweenFailureNotifications)
            .InclusiveBetween(1, 60);

        RuleFor(prop => prop.HealthChecksUI.HealthChecks)
            .Must(p => p.Count > 0)
            .WithMessage("At least 1 health check must be defined");

        RuleFor(prop => prop.Logging.LogLevel)
            .IsInEnum();

    }

    private static bool BeValidDbConnectionString(string connectionString)
    {
        return new DbConnectionString(connectionString).IsReachable;
    }
}

