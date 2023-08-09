namespace CleanArchitecture.DDD.Core.ExtensionMethods;

public static class IntegerExtensions
{
    public static double GetPercentageOf(this int num, int total)
    {
        return Math.Round(num / (double) total * 100, 2);
    }
}

