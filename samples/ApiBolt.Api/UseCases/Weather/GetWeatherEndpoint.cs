using ApiBolt.Abstractions;
using ApiBolt.AspNetCore.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace ApiBolt.Api.UseCases.Weather;

internal class GetWeatherEndpoint : IApiEndpoint, IApiEndpointConvention
{
    [HttpGet("/weather"), ApiEndpoint]
    public string GetWeatherAsync()
    {
        return $"Current weather in AMS is 14 degrees celsius";

    }
    public static void Configure(IEndpointConventionBuilder builder)
    {
        builder.RequireAuthorization(policy => policy.RequireAuthenticatedUser());
    }
}

