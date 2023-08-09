using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CleanArchitecture.DDD.Core.ExtensionMethods;

public static class AsyncEnumerableExtensions
{
    /// <summary>
    /// Source: https://stackoverflow.com/a/67676374 ::
    /// Splits the elements of a sequence into chunks that are emitted when either
    /// they are full, or a given amount of time has elapsed after requesting the
    /// previous chunk.
    /// </summary>
    public static async IAsyncEnumerable<IList<TSource>> Buffer<TSource>(
        this IAsyncEnumerable<TSource> source, 
        TimeSpan timeSpan, 
        int count,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentOutOfRangeException.ThrowIfLessThan(timeSpan, TimeSpan.FromMilliseconds(1.0));
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 1);

        using CancellationTokenSource linkedCts = CancellationTokenSource
            .CreateLinkedTokenSource(cancellationToken);
        PeriodicTimer timer = null;
        Task<bool> StartTimer()
        {
            timer = new(timeSpan);
            return timer.WaitForNextTickAsync().AsTask();
        }
        IAsyncEnumerator<TSource> enumerator = source
            .GetAsyncEnumerator(linkedCts.Token);
        Task<bool> moveNext = null;
        try
        {
            List<TSource> buffer = new();
            TSource[] ConsumeBuffer()
            {
                timer?.Dispose();
                TSource[] array = buffer.ToArray();
                buffer.Clear();
                if (buffer.Capacity > count) buffer.Capacity = count;
                return array;
            }
            Task<bool> timerTickTask = StartTimer();
            while (true)
            {
                if (moveNext is null)
                {
                    if (timerTickTask.IsCompleted)
                    {
                        Debug.Assert(timerTickTask.Result);
                        yield return ConsumeBuffer();
                        timerTickTask = StartTimer();
                    }
                    moveNext = enumerator.MoveNextAsync().AsTask();
                }
                if (!moveNext.IsCompleted)
                {
                    Task completedTask = await Task.WhenAny(moveNext, timerTickTask)
                        .ConfigureAwait(false);
                    if (ReferenceEquals(completedTask, timerTickTask))
                    {
                        Debug.Assert(timerTickTask.IsCompleted);
                        Debug.Assert(timerTickTask.Result);
                        yield return ConsumeBuffer();
                        timerTickTask = StartTimer();
                        continue;
                    }
                }
                Debug.Assert(moveNext.IsCompleted);
                bool moved = await moveNext.ConfigureAwait(false);
                moveNext = null;
                if (!moved) break;
                TSource item = enumerator.Current;
                buffer.Add(item);
                if (buffer.Count == count)
                {
                    yield return ConsumeBuffer();
                    timerTickTask = StartTimer();
                }
            }
            if (buffer.Count > 0) yield return ConsumeBuffer();
        }
        finally
        {
            // Cancel the enumerator, for more responsive completion.
            try { linkedCts.Cancel(); }
            finally
            {
                // The last moveNext must be completed before disposing.
                if (moveNext is not null && !moveNext.IsCompleted)
                    await Task.WhenAny(moveNext).ConfigureAwait(false);
                await enumerator.DisposeAsync().ConfigureAwait(false);
                timer?.Dispose();
            }
        }
    }
}

