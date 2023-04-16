using CleanArchitecture.DDD.Core.Scrutor.Attributes;

namespace CleanArchitecture.DDD.Application.ExtensionMethods;

public static class ServiceCollectionScrutorExtensions
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
    /// <param name="lifeTime"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterServicesFromAssembly<T>(
        this IServiceCollection serviceCollection,
        string endMarker = "Service",
        IEnumerable<Type>? excludedTypes = null,
        ServiceLifetime lifeTime = ServiceLifetime.Transient)
    {
        excludedTypes ??= Enumerable.Empty<Type>();

        return serviceCollection.Scan(scan =>
            scan.FromAssemblyOf<T>()
                .AddClasses(classes => classes.Where(type => type.Name.ToLower().EndsWith(endMarker.ToLower())
                                    && !excludedTypes.GetType().Name.ToLower().Contains(type.Name.ToLower())))
                .AsImplementedInterfaces()
                .WithLifetime(lifeTime)
        );
    }

    /// <summary>
    /// Registers the service by attribute
    /// Reference- https://alimozdemir.medium.com/better-di-service-registration-with-assembly-scan-3e0329e8e30a
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="serviceCollection"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterServiceByAttribute<T>(this IServiceCollection serviceCollection)
    {
        return serviceCollection.Scan(scan =>
        {
            scan.FromAssemblyOf<T>()
                .AddClasses(classes => classes.WithAttribute<TransientServiceAttribute>())
                .AsImplementedInterfaces()
                .WithTransientLifetime();

            scan.FromAssemblyOf<T>()
                .AddClasses(classes => classes.WithAttribute<ScopedServiceAttribute>())
                .AsImplementedInterfaces()
                .WithScopedLifetime();
            
            scan.FromAssemblyOf<T>()
                .AddClasses(classes => classes.WithAttribute<SingletonServiceAttribute>())
                .AsImplementedInterfaces()
                .WithSingletonLifetime();
        });


    }

}