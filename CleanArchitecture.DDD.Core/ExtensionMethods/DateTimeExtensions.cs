namespace CleanArchitecture.DDD.Core.ExtensionMethods;

public static class DateTimeExtensions
{
    public static string ToLocalDateTime(this DateTime dateTime)
    {
        return dateTime.ToString("dd.MM.yyyy HH:mm:ss");
    }
}