<img src="https://raw.githubusercontent.com/mvput/ApiBolt/main/logo.png" alt="drawing" width="200"/>

ApiBolt uses incremental source generators to generate Minimal API mapping for endpoints as Request-Endpoint-Response (REPR pattern).


# Installing

To use the ApiBolt source generator, install the `ApiBolt.AspNetCore` package and the `ApiBolt.SourceGenerator` package into your API project.

```powershell
dotnet add package ApiBolt.AspNetCore --version 0.2.1
dotnet add package ApiBolt.SourceGenerator --version 0.2.1
```

# Usage

To use the ApiBolt package, register it to your API project.

Register the services and pass the assembly where the endpoints are located, in this case inside the API assembly.

```c#
builder.Services.AddApiBolt(typeof(Program).Assembly);
```

Map the endpoints

```c#
app.MapApiBolt();
```

Create an endpoint, add the `IApiEndpoint` interface to it. This is used for DI registration. All services endpoints are registered as Scoped service. 

Create a method which represents your API method, add the `ApiEndpointAttribute` on the method. Specify the type of the call (GET, POST, PUT, DELETE) and the pattern for the API endpoint.


## Get

```c#
internal class GetWeatherEndpoint : IApiEndpoint
{
    [HttpGet("/weather"), ApiEndpoint]
    public string GetWeatherAsync()
    {
        return $"Current weather in AMS is 14 degrees celsius";
    }
}
```

For above example, the source generator creates the following output. 

```c#
public class GetWeatherEndpointRegistration : IApiEndpointRegistration
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/weather", ([FromServices] GetWeatherEndpoint endpoint) => endpoint.GetWeatherAsync());
    }
}
```

All the parameters passed into the method, are mapped to the API call. So for example

```c#
internal class GetWeatherForCityEndpoint : IApiEndpoint
{
    [HttpGet("/weather/{city}"), ApiEndpoint]
    public string GetWeatherAsync(string city, [FromQuery] int temp)
    {
        return $"Current weather in {city} is {temp} degrees celsius";
    }
}
```

Generates the following output.

```c#
public class GetWeatherForCityEndpointRegistration : IApiEndpointRegistration
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/weather/{city}", (string city, [FromQuery] int temp, [FromServices] GetWeatherForCityEndpoint endpoint) => endpoint.GetWeatherAsync(city, temp));
    }
}
```
To configure the endpoint after the Map, add the `IApiEndpointConvention` interface and add the `` method
```c#
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
```

Generates the following output.
```c#
public class GetWeatherEndpointRegistration : IApiEndpointRegistration
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var builder = app.MapGet("/weather", ([FromServices] GetWeatherEndpoint endpoint) => endpoint.GetWeatherAsync());
        GetWeatherEndpoint.Configure(builder);
    }
}
```