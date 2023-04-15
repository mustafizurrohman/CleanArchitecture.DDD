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

        LogExecutionTime(stopwatch);

        stopwatch.Reset();
    }

    public static async Task BenchmarkAsync(Func<Task> action)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        await action();
        stopwatch.Stop();

        LogExecutionTime(stopwatch);

        stopwatch.Reset();
    }

    private static void  LogExecutionTime(Stopwatch stopwatch)
    {
        LogWithSpace(() => Console.WriteLine($"Execution took {stopwatch.ElapsedMilliseconds} ms / {stopwatch.ElapsedTicks} ticks."));
    }

}
