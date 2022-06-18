using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace CleanArchitecture.DDD.Application.ExtensionMethods;

// TODO: Implement and test this!
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

    public static IServiceCollection RegisterServicesFromAssemblyWithTransientLifetime<T>(
        this IServiceCollection serviceCollection,
        string endMarker = "Service",
        IEnumerable<Type>? excludedTypes = null)
    {
        excludedTypes ??= Enumerable.Empty<Type>();

        return serviceCollection.Scan(scan =>
            scan.FromAssemblyOf<T>()
                .AddClasses(classes => classes.Where(type => type.Name.EndsWith(endMarker)
                    && !excludedTypes.DefaultIfEmpty().GetType().Name.ToLower().Contains(type.Name.ToLower())))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
        );
    }

}