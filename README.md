# ApiBolt

ApiBolt uses incremental source generators to generate Minimal API mapping for endpoints as Request-Endpoint-Response (REPR pattern).


# Installing

To use the ApiBolt source generator, install the `ApiBolt.AspNetCore` package and the `ApiBolt.SourceGenerator` package into your API project.

```powershell
dotnet add package ApiBolt.AspNetCore --version 0.0.1-beta.1
dotnet add package ApiBolt.SourceGenerator --version 0.0.1-beta.1
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
    [ApiEndpoint(ApiEndpointType.Get, "/weather")]
    public string GetWeatherAsync()
    {
        return "Current weather is 14 degrees celsius";
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
internal class GetWeatherEndpoint : IApiEndpoint
{
    [ApiEndpoint(ApiEndpointType.Get, "/weather/{city}")]
    public string GetWeatherAsync(string city, [FromQuery] int temp)
    {
        return $"Current weather in {city} is {temp} degrees celsius";
    }
}
```

Generates the following output.

```c#
public class GetWeatherEndpointRegistration : IApiEndpointRegistration
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/weather/{city}", (string city, [FromQuery] int temp, [FromServices] GetWeatherEndpoint endpoint) => endpoint.GetWeatherAsync(city, temp));
    }
}
```

