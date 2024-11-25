using Microsoft.AspNetCore.Mvc;

namespace ApiBolt.Api.Core.Models;

public class WeatherRequest
{
    [FromQuery]
    public string? City { get; init; }
}