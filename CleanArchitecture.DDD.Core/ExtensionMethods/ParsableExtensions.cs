namespace CleanArchitecture.DDD.Core.ExtensionMethods;

public static class ParsableExtensions
{
    public static T Parse<T>(this string input, IFormatProvider? formatProvider = null)
        where T : IParsable<T>
    {
        return T.Parse(input, formatProvider);
    }

    public static bool TryParse<T>(this string input, out T? result, IFormatProvider? formatProvider = null)
        where T : IParsable<T>
    {
        var parsedSuccessful = T.TryParse(input, formatProvider, out T? parsedResult);
        result = parsedResult ?? default;
        
        return parsedSuccessful;
    }
}

