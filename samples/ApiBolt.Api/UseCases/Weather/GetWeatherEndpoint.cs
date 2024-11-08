using ApiBolt.Abstractions;
using ApiBolt.AspNetCore.Abstractions;

namespace ApiBolt.Api.UseCases.Weather;

internal class GetWeatherEndpoint : IApiEndpoint, IApiEndpointConvention
{
    [ApiEndpoint(ApiEndpointType.Get, "/weather")]
    public string GetWeatherAsync()
    {
        return $"Current weather in AMS is 14 degrees celsius";

    }
    public static void Configure(IEndpointConventionBuilder builder)
    {
        builder.RequireAuthorization(policy => policy.RequireAuthenticatedUser());
    }
}

