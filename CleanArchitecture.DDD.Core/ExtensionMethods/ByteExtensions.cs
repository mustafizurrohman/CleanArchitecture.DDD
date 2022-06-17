namespace CleanArchitecture.DDD.Core.ExtensionMethods;

public static class ByteExtensions
{
    public static string AsString(this byte[] input)
    {
        return Encoding.Default.GetString(input);
    }

    public static string AsBase64String(this byte[] input)
    {
        return Convert.ToBase64String(input);
    }

    public static byte[] ByeArrayFromBase64String(this string base64String)
    {
        return Convert.FromBase64String(base64String);
    }
}