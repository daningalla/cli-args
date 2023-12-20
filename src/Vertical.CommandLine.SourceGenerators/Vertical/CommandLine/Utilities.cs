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

    internal static string FormatArgumentProviderCall(IMemberMetadata member)
    {
        var elementType = member.CollectionElementType;
        var quotedId = $"\"{member.BindingId}\"";

        return member.CollectionType switch
        {
            CollectionType.Array => $"GetValueArray<{elementType}>({quotedId})",
            CollectionType.List => $"GetValueList<{elementType}>({quotedId})",
            CollectionType.LinkedList => $"GetValueLinkedList<{elementType}>({quotedId})",
            CollectionType.HashSet => $"GetValueHashSet<{elementType}>({quotedId})",
            CollectionType.SortedSet => $"GetValueSortedSet<{elementType}>({quotedId})",
            CollectionType.Stack => $"GetValueStack<{elementType}>({quotedId})",
            CollectionType.Queue => $"GetValueQueue<{elementType}>({quotedId})",
            _ => $"GetValue<{member.Type}>({quotedId})"
        };
    }

    internal static string? GetExplicitSymbolBindingId(ISymbol symbol)
    {
        var bindingAttribute = symbol
            .GetAttributes()
            .FirstOrDefault(attributeData => attributeData is
            {
                AttributeClass:
                {
                    Name: Constants.BindingAttributeName,
                    ContainingAssembly.Name: Constants.CommandLineAssembly
                }
            });

        var constructorArguments = bindingAttribute?.ConstructorArguments;
        var idArgument = constructorArguments?.FirstOrDefault();
        return idArgument?.Value as string;
    }
    
    internal static CollectionType GetSymbolCollectionType(ITypeSymbol typeSymbol)
    {
        if (typeSymbol.SpecialType == SpecialType.System_Array)
            return CollectionType.Array;

        var fullTypeName = typeSymbol.ToDisplayString();

        if (!fullTypeName.StartsWith("System.Collections.Generic"))
            return CollectionType.None;

        var typeName = typeSymbol.Name;
        
        // Best guess on most common types
        if (typeName.StartsWith("List")) return CollectionType.List;
        if (typeName.StartsWith("IList")) return CollectionType.Array;
        if (typeName.StartsWith("IEnumerable")) return CollectionType.Array;
        if (typeName.StartsWith("ICollection")) return CollectionType.List;
        if (typeName.StartsWith("IReadOnlyList")) return CollectionType.Array;
        if (typeName.StartsWith("IReadOnlyCollection")) return CollectionType.List;
        
        // Sets
        if (typeName.StartsWith("HashSet")) return CollectionType.HashSet;
        if (typeName.StartsWith("ISet")) return CollectionType.HashSet;
        if (typeName.StartsWith("IReadOnlySet")) return CollectionType.HashSet;
        if (typeName.StartsWith("SortedSet")) return CollectionType.SortedSet;
        
        // Other
        if (typeName.StartsWith("Stack")) return CollectionType.Stack;
        if (typeName.StartsWith("Queue")) return CollectionType.Queue;
        if (typeName.StartsWith("LinkedList")) return CollectionType.LinkedList;

        return CollectionType.None;
    }

    internal static string EnsureCamelCase(string str)
    {
        if (str.Length == 0) return str;

        if (char.IsLower(str[0])) return str;

        return str.Length > 1
            ? $"{char.ToLower(str[0])}{str.Substring(1)}"
            : char.ToLower(str[0]).ToString();
    }
}