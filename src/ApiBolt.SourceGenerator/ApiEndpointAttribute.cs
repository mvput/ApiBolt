namespace ApiBolt.SourceGenerator;

[AttributeUsage(AttributeTargets.Method)]
public class ApiEndpointAttribute(ApiEndpointType apiEndpointType, string pattern) : Attribute
{
    public ApiEndpointType ApiEndpointType { get; } = apiEndpointType;
    public string Pattern { get; } = pattern;
}