using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

namespace Vertical.CommandLine;

[Generator]
public sealed class  CommandHandlerGenerator : IIncrementalGenerator
{
    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var diagnostics = new List<Diagnostic>();
        
        var handlers = context
            .SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (node, _) => IsHandlerAssignmentSyntax(node),
                transform: (syntaxContext, _) => CreateHandlerMetadata(syntaxContext, diagnostics))
            .Where(x => x is not null);

        var collected = context
            .CompilationProvider
            .Combine(handlers.Collect());
        
        context.RegisterSourceOutput(collected, (prodContext, source) =>
        {
            var referenceHandlers = source
                .Right
                .Where(item => item is not null)
                .Cast<HandlerMetadata>()
                .ToArray();
            
            PostValidateProviderValueSource(referenceHandlers, diagnostics);

            foreach (var diagnostic in diagnostics)
            {
                prodContext.ReportDiagnostic(diagnostic);
            }

            var builder = new HandlerSourceBuilder(source.Left.Options.OptimizationLevel, diagnostics.Count > 0);
            var sourceContent = builder.Build(referenceHandlers);
            
            prodContext.AddSource(Constants.FileNameHint, sourceContent);
        });
    }

    private static bool IsHandlerAssignmentSyntax(SyntaxNode node)
    {
        if (node is not AssignmentExpressionSyntax
            {
                Left: IdentifierNameSyntax { Identifier.Text: Constants.HandlerIdentifier }
            } assignmentExpr)
        {
            return false;
        }

        return assignmentExpr.Parent is InitializerExpressionSyntax;
    }

    private static HandlerMetadata? CreateHandlerMetadata(
        GeneratorSyntaxContext context,
        ICollection<Diagnostic> diagnostics)
    {
        var assignmentExpressionSyntax = (AssignmentExpressionSyntax)context.Node;
        var initializerExpressionSyntax = assignmentExpressionSyntax.Parent as InitializerExpressionSyntax;
        var objectCreationExpressionSyntax = initializerExpressionSyntax?.Parent as ObjectCreationExpressionSyntax;
        var typeSyntax = objectCreationExpressionSyntax?.Type;

        if (typeSyntax == null)
            return null;

        var typeInfoToken = context.SemanticModel.GetSymbolInfo(typeSyntax);

        if (!typeInfoToken.Symbol.TryGetCommandSymbol(out var commandTypeSymbol))
            return null;

        // Find the implementation method
        var handlerMethodSymbol = GetHandlerImplementationMethodSymbol(
            context.SemanticModel,
            assignmentExpressionSyntax.Right);

        if (handlerMethodSymbol == null)
            return null;

        var isRootCommand = commandTypeSymbol!.Name == Constants.RootCommandClass;
        var commandId = GetCommandId(context.SemanticModel, objectCreationExpressionSyntax!);

        if (string.IsNullOrWhiteSpace(commandId) && !isRootCommand)
        {
            diagnostics.Add(Diagnostics.Vcl_001(assignmentExpressionSyntax));
            return null;
        }

        var parameters = handlerMethodSymbol
            .Parameters
            .Select(parameterSymbol => new ParameterMetadata(parameterSymbol))
            .ToArray();
        
        return new HandlerMetadata(
            commandId!,
            isRootCommand,
            handlerMethodSymbol,
            parameters);
    }

    private static void PostValidateProviderValueSource(
        HandlerMetadata[] handlers,
        ICollection<Diagnostic> diagnostics)
    {
        if (handlers.Count(handler => handler.IsRootCommand) > 1)
        {
            diagnostics.Add(Diagnostics.Vcl_002());
        }

        var returnTypeHashSet = new HashSet<ITypeSymbol>(
            handlers.Select(handler => handler.ReturnType),
            SymbolEqualityComparer.IncludeNullability);

        if (returnTypeHashSet.Count > 1)
        {
            diagnostics.Add(Diagnostics.Vcl_003(handlers));
        }

        var uniqueCommandIds = handlers
            .Select(handler => handler.CommandId)
            .GroupBy(handler => handler)
            .Select(grouping => new
            {
                grouping.Key,
                Count = grouping.Count()
            })
            .Where(item => item.Count > 1)
            .Select(item => item.Key)
            .ToArray();

        if (uniqueCommandIds.Length > 0)
        {
            diagnostics.Add(Diagnostics.Vcl_004(uniqueCommandIds));
        }
    }

    private static IMethodSymbol? GetHandlerImplementationMethodSymbol(
        SemanticModel semanticModel,
        ExpressionSyntax expressionSyntax)
    {
        switch (expressionSyntax)
        {
            // Is this a method reference?
            case IdentifierNameSyntax identifierNameSyntax:
                return semanticModel.GetOperation(identifierNameSyntax) is IMethodReferenceOperation
                    methodReferenceOperation
                    ? methodReferenceOperation.Method
                    : null;
            
            case ParenthesizedLambdaExpressionSyntax lambdaExpressionSyntax:
                return semanticModel.GetOperation(lambdaExpressionSyntax) is IAnonymousFunctionOperation
                    anonymousFunctionOperation
                    ? anonymousFunctionOperation.Symbol
                    : null;
        }

        return null;
    }

    private static string? GetCommandId(
        SemanticModel semanticModel,
        ObjectCreationExpressionSyntax commandCreationSyntax)
    {
        var argumentSyntax = commandCreationSyntax
            .ArgumentList?
            .Arguments
            .FirstOrDefault();

        var expressionSyntax = argumentSyntax?.Expression;

        switch (expressionSyntax)
        {
            case null:
                return Constants.RootCommandId;

            case LiteralExpressionSyntax literalExpressionSyntax:
                return literalExpressionSyntax.Token.ValueText;

            default:
                return semanticModel.GetExpressionValueText(argumentSyntax!.Expression);
        }
    }
}