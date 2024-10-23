// Copyright (c) GRCcontrol B.V. All rights reserved.

using ApiBolt.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace ApiBolt.Api.UseCases.Weather;

internal class GetWeatherEndpoint : IApiEndpoint
{
    [ApiEndpoint(ApiEndpointType.Get, "/weather/{city}")]
    public string GetWeatherAsync(string city, [FromQuery] int temp)
    {
        return $"Current weather in {city} is {temp} degrees celsius";
    }
}