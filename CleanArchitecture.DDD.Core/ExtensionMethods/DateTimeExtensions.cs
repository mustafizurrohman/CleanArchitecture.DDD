namespace CleanArchitecture.DDD.Core.ExtensionMethods;

public static class DateTimeExtensions
{
    // ReSharper disable once InconsistentNaming
    public static string ToLocalDEDateTime(this DateTime dateTime)
    {
        return dateTime.ToString("dd.MM.yyyy HH:mm:ss");
    }
}