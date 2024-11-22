// Copyright (c) GRCcontrol B.V. All rights reserved.

using System.Reflection;
using ApiBolt.Abstractions;
using ApiBolt.AspNetCore.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace ApiBolt.AspNetCore;

public static class ServiceCollectionExtensions
{
    public static void AddApiBolt(this IServiceCollection services, Assembly assembly)
    {
        services.Scan(scan => scan.FromAssemblies(assembly).AddClasses(c => c.AssignableTo<IApiEndpoint>())
            .AsSelfWithInterfaces());

        services.Scan(scan => scan.FromAssemblies(assembly).AddClasses(c => c.AssignableTo<IApiEndpointRegistration>())
            .AsImplementedInterfaces());
    }

    public static void AddApiBolt<T>(this IServiceCollection services) => services.AddApiBolt(typeof(T).Assembly);
}