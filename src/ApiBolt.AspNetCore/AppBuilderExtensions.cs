using ApiBolt.Abstractions;
using ApiBolt.AspNetCore.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ApiBolt.AspNetCore;

public static class AppBuilderExtensions
{
    public static void MapApiBolt(this WebApplication app)
    {
        foreach (var endpoint in app.Services.GetServices<IApiEndpointRegistration>())
        {
            endpoint.MapEndpoint(app);
        }
    }
}