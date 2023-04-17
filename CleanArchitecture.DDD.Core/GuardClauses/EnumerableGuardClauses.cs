namespace CleanArchitecture.DDD.Core.GuardClauses;

public static class EnumerableGuardClauses
{
    public static void EmptyOrNullEnumerable<T>(this IGuardClause guardClause, IEnumerable<T> collection)
        where T : class
    {
        if (collection is null || !collection.Any())
        {
            throw new ArgumentException("Enumerable must contain elements");
        }
    }
}