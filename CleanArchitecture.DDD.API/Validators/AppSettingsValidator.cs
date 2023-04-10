using CleanArchitecture.DDD.API.Models;
using Microsoft.Data.SqlClient;
using System.Data;

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
            .GreaterThanOrEqualTo(1);

        RuleFor(prop => prop.HealthChecksUI.MinimumSecondsBetweenFailureNotifications)
            .GreaterThanOrEqualTo(1);

        RuleFor(prop => prop.HealthChecksUI.HealthChecks)
            .Must(p => p.Count > 0)
            .WithMessage("At least 1 health check must be defined");

        RuleFor(prop => prop.Logging.LogLevel)
            .IsInEnum();

    }

    private static bool BeValidDbConnectionString(string connectionString)
    {
        static string RemoveDatabaseFromConnectionString(string connStr)
        {
            return connStr
                .Split(";")
                .Where(str => !str.Contains("Database="))
                .Aggregate((a, b) => a + ";" + b);
        }

        if (string.IsNullOrWhiteSpace(connectionString))
            return false;

        connectionString = RemoveDatabaseFromConnectionString(connectionString);

        SqlConnection? connection = null;

        try
        {
            connection = new SqlConnection(connectionString);
            connection.Open();
        }
        catch (Exception)
        {
            return false;
        }
        finally
        {
            if (connection?.State == ConnectionState.Open)
                connection.Close();
        }

        return true;
    }
}

