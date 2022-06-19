namespace CleanArchitecture.DDD.Application.ExtensionMethods;

public static class ServiceCollectionExtensions
{
    private static IServiceCollection RegisterClassesFromAssemblyWithTransientLifetime<T>(
        this IServiceCollection serviceCollection,
        Action<IImplementationTypeFilter> filter)
    {
        return serviceCollection.Scan(scan =>
            scan.FromAssemblyOf<T>()
                .AddClasses(filter)
                .AsImplementedInterfaces()
                .WithTransientLifetime()
        );
    }

    // Scrutor Will slightly increase the application startup time because it uses reflection
    // Reference- https://andrewlock.net/using-scrutor-to-automatically-register-your-services-with-the-asp-net-core-di-container/

    /// <summary>
    /// Register all services (using Scrutor) with name ending with 'Service' or a specified string 
    /// in a specified assembly optionally excluding specific types 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="serviceCollection"></param>
    /// <param name="endMarker"></param>
    /// <param name="excludedTypes"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterServicesFromAssemblyWithTransientLifetime<T>(
        this IServiceCollection serviceCollection,
        string endMarker = "Service",
        IEnumerable<Type>? excludedTypes = null)
    {
        excludedTypes ??= Enumerable.Empty<Type>();

        return serviceCollection.Scan(scan =>
            scan.FromAssemblyOf<T>()
                .AddClasses(classes => classes.Where(type => type.Name.EndsWith(endMarker)
                                    && !excludedTypes.GetType().Name.ToLower().Contains(type.Name.ToLower())))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
        );
    }

}