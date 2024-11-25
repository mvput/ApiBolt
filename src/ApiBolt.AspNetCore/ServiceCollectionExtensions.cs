using System.Reflection;
using ApiBolt.Abstractions;
using ApiBolt.AspNetCore.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace ApiBolt.AspNetCore;

public static class ServiceCollectionExtensions
{
    private static void AddApiBolt(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.Scan(scan => scan.FromAssemblies(assemblies).AddClasses(c => c.AssignableTo<IApiEndpoint>())
            .AsSelfWithInterfaces());

        services.Scan(scan => scan.FromAssemblies(assemblies).AddClasses(c => c.AssignableTo<IApiEndpointRegistration>())
            .AsImplementedInterfaces());
    }

    public static void AddApiBolt(this IServiceCollection services) => services.AddApiBolt(Assembly.GetCallingAssembly());
    public static void AddApiBolt<T>(this IServiceCollection services) => services.AddApiBolt(typeof(T).Assembly);
}