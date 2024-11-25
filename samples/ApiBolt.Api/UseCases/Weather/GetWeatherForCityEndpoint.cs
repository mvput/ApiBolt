using ApiBolt.Abstractions;
using ApiBolt.Api.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiBolt.Api.UseCases.Weather;

internal class GetWeatherForCityEndpoint : IApiEndpoint
{
    [HttpGet("/weather"), ApiEndpoint]
    public string GetAsync([AsParameters] WeatherRequest city, [FromQuery] int temp)
    {
        return $"Current weather in {city.City} is {temp} degrees celsius";
    }
}