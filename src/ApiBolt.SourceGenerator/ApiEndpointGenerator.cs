﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace ApiBolt.SourceGenerator;

[Generator]
public class ApiEndpointGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var endpoints = context.SyntaxProvider.ForAttributeWithMetadataName("ApiBolt.ApiEndpointAttribute",
                (node, _) => node is MethodDeclarationSyntax,
                static (ctx, _) => GetApiToGenerate(ctx))
            .Where(static m => m is not null);

        context.RegisterSourceOutput(endpoints,
            static (spc, source) => Execute(source, spc));
    }

    private static ApiEndpointToGenerate? GetApiToGenerate(GeneratorAttributeSyntaxContext context)
    {
        var methodDeclarationSyntax = (MethodDeclarationSyntax)context.TargetNode;

        if (methodDeclarationSyntax.Parent is null)
        {
            return null;
        }

        var classDeclarationSyntax = (ClassDeclarationSyntax)methodDeclarationSyntax.Parent;

        if (context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax) is not { } namedTypeSymbol)
            return null;

        var endpointName = namedTypeSymbol.Name;
        var attributes = GetAttribute(methodDeclarationSyntax);

        if (attributes is null)
        {
            return null;
        }
        
        var hasConvention = namedTypeSymbol.Interfaces.Any(static n => n.Name == "IApiEndpointConvention");
        var name = methodDeclarationSyntax.Identifier.ValueText;

        return new ApiEndpointToGenerate(endpointName, GetUsings(namedTypeSymbol), GetNamespace(classDeclarationSyntax), attributes.ApiEndpointType, attributes.Pattern, name, hasConvention, methodDeclarationSyntax.ParameterList); ;
    }

    private static ApiEndpointAttribute? GetAttribute(MethodDeclarationSyntax methodDeclarationSyntax)
    {
        foreach (var attributeSyntax in methodDeclarationSyntax.AttributeLists.SelectMany(attributelistSyntax => attributelistSyntax.Attributes))
        {
            if (attributeSyntax.ArgumentList is null) continue;

            var pattern = attributeSyntax.ArgumentList.Arguments.First();

            return new ApiEndpointAttribute((ApiEndpointType)Enum.Parse(typeof(ApiEndpointType), attributeSyntax.Name.ToFullString().Replace("Http", string.Empty)), pattern.Expression.NormalizeWhitespace().ToFullString());
        }

        return null;
    }

    private static void Execute(ApiEndpointToGenerate? apiEndpointToGenerate, SourceProductionContext context)
    {
        if (apiEndpointToGenerate is not { } value) return;
        var result = SourceGenerationHelper.GenerateRegistrationClass(value);
        context.AddSource($"{value.Ns}/{value.Name}Registration.g.cs", SourceText.From(result, Encoding.UTF8));
    }

    private static string GetNamespace(BaseTypeDeclarationSyntax syntax)
    {
        var nameSpace = string.Empty;

        var potentialNamespaceParent = syntax.Parent;

        while (potentialNamespaceParent != null &&
               potentialNamespaceParent is not NamespaceDeclarationSyntax
               && potentialNamespaceParent is not FileScopedNamespaceDeclarationSyntax)
            potentialNamespaceParent = potentialNamespaceParent.Parent;

        if (potentialNamespaceParent is not BaseNamespaceDeclarationSyntax namespaceParent) return nameSpace;
        nameSpace = namespaceParent.Name.ToString();

        while (true)
        {
            if (namespaceParent.Parent is not NamespaceDeclarationSyntax parent) break;

            nameSpace = $"{namespaceParent.Name}.{nameSpace}";
            namespaceParent = parent;
        }

        return nameSpace;
    }

    private static UsingDirectiveSyntax[] GetUsings(INamedTypeSymbol symbol)
    {
        var allUsings = SyntaxFactory.List<UsingDirectiveSyntax>();
        allUsings = symbol.DeclaringSyntaxReferences.SelectMany(syntaxRef => syntaxRef.GetSyntax().Ancestors(false))
            .Aggregate(allUsings, (current, parent) => parent switch
            {
                NamespaceDeclarationSyntax syntax => current.AddRange(syntax.Usings),
                CompilationUnitSyntax syntax => current.AddRange(syntax.Usings),
                _ => current
            });
        return allUsings.ToArray();
    }
}