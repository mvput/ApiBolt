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
                    IdentifierName($"Map{value.EndpointType.ToString()}")));

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
                    TriviaList()))).WithVariables(SingletonSeparatedList(
                VariableDeclarator(
                        Identifier("builder"))
                    .WithInitializer(
                        EqualsValueClause(invocation)))));
            
            
            
            var configure =    ExpressionStatement(
                InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(value.Name),
                            IdentifierName("Configure")))
                    .WithArgumentList(
                        ArgumentList(
                            SingletonSeparatedList(
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
            .AddUsings(GetUsings(value.Usings));

        return root
            .NormalizeWhitespace()
            .ToFullString();
    }

    private static ArgumentListSyntax ToArguments(ParameterListSyntax parameter)
    {
        var arguments = parameter.Parameters.Select(x => Argument(IdentifierName(x.Identifier.ValueText)));
        return ArgumentList(SeparatedList(arguments));
    }

    private static UsingDirectiveSyntax[] GetUsings(UsingDirectiveSyntax[] valueUsings)
    {
        var usings = valueUsings.ToList();
        usings.AddRange(
            [
            UsingDirective(ParseName("ApiBolt.AspNetCore.Abstractions")),
            UsingDirective(ParseName("Microsoft.AspNetCore.Mvc")),
            UsingDirective(ParseName("Microsoft.AspNetCore.Routing"))]
            );

        return usings.OrderBy(x => x.Name?.ToFullString()).DistinctBy(x => x.Name?.ToFullString()).ToArray();
    }

  
}

public static class SyntaxExtensions
{
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>
        (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        HashSet<TKey> seenKeys = [];
        foreach (var element in source)
        {
            if (seenKeys.Add(keySelector(element)))
            {
                yield return element;
            }
        }
    }}