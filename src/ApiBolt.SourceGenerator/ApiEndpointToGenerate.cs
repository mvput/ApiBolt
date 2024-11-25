using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ApiBolt.SourceGenerator;

public readonly record struct ApiEndpointToGenerate(
    string Name,
    UsingDirectiveSyntax[] Usings,
    string Ns,
    ApiEndpointType EndpointType,
    string Pattern,
    string Method,
    bool HasConvention,
    ParameterListSyntax Parameter)
{
    public readonly string Name = Name;
    public readonly string Ns = Ns;
    public readonly UsingDirectiveSyntax[] Usings = Usings;
    public readonly ApiEndpointType EndpointType = EndpointType;
    public readonly string Pattern = Pattern;
    public readonly bool HasConvention = HasConvention;
    public readonly string Method = Method;
    public readonly ParameterListSyntax Parameter = Parameter;
}