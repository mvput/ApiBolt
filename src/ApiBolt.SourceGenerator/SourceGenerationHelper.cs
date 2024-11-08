using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace ApiBolt.SourceGenerator;

public static class SourceGenerationHelper
{
    public static string GenerateRegistrationClass(ApiEndpointToGenerate value)
    {
        var invocation = InvocationExpression(MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName("app"),
                    IdentifierName(GetMethodNameForEndpointType(value.EndpointType))));

       var statements = new List<StatementSyntax>();

        invocation = invocation.WithArgumentList
            (
            ArgumentList(
                   SeparatedList<ArgumentSyntax>(new SyntaxNodeOrToken[]
                   {
                       Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, ParseToken(value.Pattern))),
                         Token(SyntaxKind.CommaToken),
                         Argument
                         (
                             ParenthesizedLambdaExpression()
                             .WithParameterList(value.Parameter
                             .AddParameters(
                                 Parameter(
                                                                    Identifier("endpoint"))
                                                                .WithAttributeLists(
                                                                    SingletonList(
                                                                        AttributeList(
                                                                            SingletonSeparatedList(
                                                                                Attribute(
                                                                                    IdentifierName("FromServices"))))))
                                                                .WithType(
                                                                    IdentifierName(value.Name))
                                 )).WithExpressionBody(
                                                        InvocationExpression(
                                                            MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                IdentifierName("endpoint"),
                                                                IdentifierName(value.Method)), ToArguments(value.Parameter) )))
                   })
                )
            );

        if (value.HasConvention)
        {
            var syntax = LocalDeclarationStatement(VariableDeclaration(IdentifierName(
                Identifier(
                    TriviaList(),
                    SyntaxKind.VarKeyword,
                    "var",
                    "var",
                    TriviaList()))).WithVariables(SingletonSeparatedList<VariableDeclaratorSyntax>(
                VariableDeclarator(
                        Identifier("builder"))
                    .WithInitializer(
                        EqualsValueClause(invocation)))));
            
            
            
            var configure =    ExpressionStatement(
                InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName("GetWeatherEndpoint"),
                            IdentifierName("Configure")))
                    .WithArgumentList(
                        ArgumentList(
                            SingletonSeparatedList<ArgumentSyntax>(
                                Argument(
                                    IdentifierName("builder"))))));

            statements.Add(syntax);
            statements.Add(configure);
        }

        else
        {
            statements.Add(ExpressionStatement(invocation));
        }
        var method = MethodDeclaration(ParseTypeName("void"), "MapEndpoint")
                       .AddModifiers(Token(SyntaxKind.PublicKeyword))
                       .WithBody(Block(statements))
                       .AddParameterListParameters(Parameter(Identifier("app")).WithType(ParseTypeName("IEndpointRouteBuilder")));

        var @class = ClassDeclaration($"{value.Name}Registration")
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .AddBaseListTypes(SimpleBaseType(ParseTypeName("IApiEndpointRegistration")))
            .AddMembers(method);

        var root = NamespaceDeclaration(ParseName(value.Ns))
            .AddMembers(@class)
            .AddUsings(
                UsingDirective(ParseName("ApiBolt.AspNetCore.Abstractions")),
                UsingDirective(ParseName("Microsoft.AspNetCore.Mvc")),
                UsingDirective(ParseName("Microsoft.AspNetCore.Routing"))
            );

        return root
            .NormalizeWhitespace()
            .ToFullString();
    }

    private static ArgumentListSyntax ToArguments(ParameterListSyntax parameter)
    {
        var arguments = parameter.Parameters.Select(x => Argument(IdentifierName(x.Identifier.ValueText)));
        return ArgumentList(SeparatedList(arguments));
    }

    private static string GetMethodNameForEndpointType(ApiEndpointType apiEndpointType)
    {
        switch (apiEndpointType)
        {
            case ApiEndpointType.Get:
                return "MapGet";

            case ApiEndpointType.Post:
                return "MapPost";

            case ApiEndpointType.Delete:
                return "MapDelete";

            case ApiEndpointType.Put:
                return "MapPut";

            default:
                return "";
        }
    }
}