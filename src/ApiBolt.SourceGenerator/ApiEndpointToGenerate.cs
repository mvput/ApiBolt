using System.Collections.Immutable;

namespace ApiBolt.SourceGenerator;

public readonly record struct ApiEndpointToGenerate(
    string Name,
    string Ns,
    ApiEndpointType EndpointType,
    string Pattern)
{
    public readonly string Name = Name;
    public readonly string Ns = Ns;
    public readonly ApiEndpointType EndpointType = EndpointType;
    public readonly string Pattern = Pattern;
}