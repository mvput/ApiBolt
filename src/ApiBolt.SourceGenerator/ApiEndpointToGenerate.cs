﻿using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ApiBolt.SourceGenerator;

public readonly record struct ApiEndpointToGenerate(
    string Name,
    string Ns,
    ApiEndpointType EndpointType,
    string Pattern,
    string Method,
    ParameterListSyntax Parameter)
{
    public readonly string Name = Name;
    public readonly string Ns = Ns;
    public readonly ApiEndpointType EndpointType = EndpointType;
    public readonly string Pattern = Pattern;
    public readonly string Method = Method;
    public readonly ParameterListSyntax Parameter = Parameter;
}