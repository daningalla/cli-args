using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;

namespace Vertical.CommandLine;

internal static class Utilities
{
    internal static string? GetExpressionValueText(
        this SemanticModel semanticModel,
        SyntaxNode syntaxNode)
    {
        var operation = semanticModel.GetOperation(syntaxNode);

        switch (operation)
        {
            case IFieldReferenceOperation fieldReferenceOperation:
                return fieldReferenceOperation.ConstantValue.Value as string;
            
            case ILocalReferenceOperation localReferenceOperation:
                return localReferenceOperation.ConstantValue.Value as string;
        }

        return null;
    }

    internal static bool TryGetCommandSymbol(this ISymbol? symbol, out ITypeSymbol? typeSymbol)
    {
        typeSymbol = symbol as ITypeSymbol;
        
        return typeSymbol is
        {
            ContainingAssembly.Name: Constants.CommandLineAssembly,
            Name: Constants.RootCommandClass or Constants.CommandClass
        };
    }
    
    internal static ITypeSymbol? GetGenericArgumentType(this ITypeSymbol typeSymbol)
    {
        return typeSymbol is INamedTypeSymbol { TypeArguments.Length: 1 } genericTypeSymbol
            ? genericTypeSymbol.TypeArguments[0]
            : null;
    }
    
    internal static string Capitalize(string str)
    {
        if (str.Length == 0 || char.IsUpper(str[0])) return str;
        
        return str.Length == 1
            ? char.ToUpper(str[0]).ToString()
            : $"{char.ToUpper(str[0])}{str.Substring(1)}";
    }
}