using System.Text;

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
}