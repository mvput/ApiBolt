using ApiBolt.Abstractions;

namespace ApiBolt.Api.UseCases.Weather;

internal class GetWeatherEndpoint : IApiEndpoint
{
    [ApiEndpoint(ApiEndpointType.Get, "/weather")]
    public string GetWeatherAsync()
    {
        return $"Current weather in AMS is 14 degrees celsius";
    }
}