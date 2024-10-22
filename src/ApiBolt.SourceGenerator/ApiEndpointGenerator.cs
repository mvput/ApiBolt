// Copyright (c) GRCcontrol B.V. All rights reserved.

using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

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


        if (context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax) is not INamedTypeSymbol namedTypeSymbol)
            return null;

        var endpointName = namedTypeSymbol.Name;
        var attributes = GetAttribute(context.SemanticModel, methodDeclarationSyntax);

        if (attributes is null)
        {
            return null;
        }

    return new ApiEndpointToGenerate(endpointName, GetNamespace(classDeclarationSyntax), attributes.ApiEndpointType, attributes.Pattern);
    }

    private static ApiEndpointAttribute? GetAttribute(SemanticModel semanticModel, MethodDeclarationSyntax methodDeclarationSyntax)
    {
        foreach (var attributeSyntax in methodDeclarationSyntax.AttributeLists.SelectMany(attributelistSyntax => attributelistSyntax.Attributes))
        {
            var arguments = attributeSyntax.ArgumentList!.Arguments!;



            return new ApiEndpointAttribute(ApiEndpointType.Get, "/riskprofile");
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
        // If we don't have a namespace at all we'll return an empty string
        // This accounts for the "default namespace" case
        var nameSpace = string.Empty;

        // Get the containing syntax node for the type declaration
        // (could be a nested type, for example)
        var potentialNamespaceParent = syntax.Parent;

        // Keep moving "out" of nested classes etc until we get to a namespace
        // or until we run out of parents
        while (potentialNamespaceParent != null &&
               potentialNamespaceParent is not NamespaceDeclarationSyntax
               && potentialNamespaceParent is not FileScopedNamespaceDeclarationSyntax)
            potentialNamespaceParent = potentialNamespaceParent.Parent;

        // Build up the final namespace by looping until we no longer have a namespace declaration
        if (potentialNamespaceParent is BaseNamespaceDeclarationSyntax namespaceParent)
        {
            // We have a namespace. Use that as the type
            nameSpace = namespaceParent.Name.ToString();

            // Keep moving "out" of the namespace declarations until we
            // run out of nested namespace declarations
            while (true)
            {
                if (namespaceParent.Parent is not NamespaceDeclarationSyntax parent) break;

                // Add the outer namespace as a prefix to the final namespace
                nameSpace = $"{namespaceParent.Name}.{nameSpace}";
                namespaceParent = parent;
            }
        }

        // return the final namespace
        return nameSpace;
    }
}