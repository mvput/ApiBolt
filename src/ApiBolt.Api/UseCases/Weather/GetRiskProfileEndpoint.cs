// Copyright (c) GRCcontrol B.V. All rights reserved.

using ApiBolt.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace ApiBolt.Api.UseCases.Weather;

internal class GetWeatherEndpoint : IApiEndpoint
{
    [ApiEndpoint(ApiEndpointType.Get, "/weather")]
    public async Task<string> GetWeatherAsync()
    {
        return $"Current weather is 14 degrees celcius";
    }
}