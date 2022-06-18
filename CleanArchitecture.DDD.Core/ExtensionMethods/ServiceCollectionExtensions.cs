using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace CleanArchitecture.DDD.Core.ExtensionMethods;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterClassesFromAssembly<T>(this IServiceCollection serviceCollection,
        Action<IImplementationTypeFilter> filter)
    {
        return serviceCollection.Scan(scan =>
            scan.FromAssemblyOf<T>()
                .AddClasses(filter)
                .AsImplementedInterfaces()
                .WithTransientLifetime()
        );
    }
}