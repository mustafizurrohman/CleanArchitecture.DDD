using System.Diagnostics;

namespace CleanArchitecture.DDD.Core.Helpers;

public static class LoggingHelper
{
    public static void LogWithSpace(Action action)
    {
        Console.WriteLine();
        action();
        Console.WriteLine();
    }
}
