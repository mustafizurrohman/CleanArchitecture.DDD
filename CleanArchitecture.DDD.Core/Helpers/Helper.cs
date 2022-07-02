using System.Diagnostics;

namespace CleanArchitecture.DDD.Core.Helpers;

public static class Helper
{
    public static void LogWithSpace(Action action)
    {
        Console.WriteLine();
        action();
        Console.WriteLine();
    }

    // TODO: Can this be implemented as an attribute?
    public static void Benchmark(Action action)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        action();
        stopwatch.Stop();

        var executionTime = stopwatch.ElapsedMilliseconds;

        LogWithSpace(() => Console.WriteLine($"Execution took {executionTime} ms."));
    }
}
