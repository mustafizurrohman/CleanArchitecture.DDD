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

    public static byte[] ByteArrayFromBase64String(this string base64String)
    {
        if (base64String.IsBase64String())
            throw new ArgumentException("Input string is not a valid base64 string");

        return Convert.FromBase64String(base64String);
    }

    public static bool IsBase64String(this string inputString)
    {
        Span<byte> buffer = new Span<byte>(new byte[inputString.Length]);
        return Convert.TryFromBase64String(inputString, buffer, out _);
    }
}