// Copyright (c) GRCcontrol B.V. All rights reserved.

using ApiBolt.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace ApiBolt.Api.UseCases.Weather;

internal class GetWeatherForCityEndpoint : IApiEndpoint
{
    [HttpGet("/weather/{city}"), ApiEndpoint]
    public string GetAsync(string city, [FromQuery] int temp)
    {
        return $"Current weather in {city} is {temp} degrees celsius";
    }
}