using Bogus;

namespace CleanArchitecture.DDD.Core.ExtensionMethods;

public static class EnumerableExtensions
{
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        return source
            .OrderBy(_ => Guid.NewGuid());
    }

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (T item in source)
        {
            action(item);
        }
    }

    public static async Task ForEachAsync<T>(this List<T> source, Func<T, Task> func)
    {
        foreach (var item in source)
        {
            await func(item);
        }
    }

    public static T GetRandomElement<T>(this IEnumerable<T> source)
    {
        return (new Faker()).Random.ArrayElement(source.ToArray());
    }

}