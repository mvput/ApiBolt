﻿// Copyright (c) GRCcontrol B.V. All rights reserved.

using System.Reflection;
using ApiBolt.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace ApiBolt;

public static class ServiceCollectionExtensions
{
    public static void AddApiBolt(this IServiceCollection services, Assembly assembly)
    {
        services.Scan(scan => scan.FromAssemblies(assembly).AddClasses(c => c.AssignableTo<IApiEndpoint>())
            .AsSelfWithInterfaces());

        services.Scan(scan => scan.FromAssemblies(assembly).AddClasses(c => c.AssignableTo<IApiEndpointRegistration>())
            .AsImplementedInterfaces());
    }
}