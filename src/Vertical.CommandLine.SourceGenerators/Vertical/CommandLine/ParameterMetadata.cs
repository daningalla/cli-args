using Microsoft.CodeAnalysis;

namespace Vertical.CommandLine;

public sealed class ParameterMetadata
{
    public ParameterMetadata(IParameterSymbol parameterSymbol)
    {
        ParameterName = parameterSymbol.Name;
        ParameterType = parameterSymbol.Type;
        BindingId = GetBindingId(parameterSymbol) ?? ParameterName;
        CollectionType = GetParameterCollectionType(parameterSymbol.Type);
        CollectionElementType = parameterSymbol.Type.GetGenericArgumentType();
        Ordinal = parameterSymbol.Ordinal;
        IsCancellationToken = parameterSymbol.Type.ToDisplayString() ==
                              "System.Threading.CancellationToken";
    }

    public bool IsCancellationToken { get; set; }

    public ParameterCollectionType CollectionType { get; set; }

    public ITypeSymbol? CollectionElementType { get; set; }

    public int Ordinal { get; set; }

    public string BindingId { get; }

    public string ParameterName { get; }

    public ITypeSymbol ParameterType { get; }

    private static string? GetBindingId(ISymbol parameterSymbol)
    {
        var bindingAttribute = parameterSymbol
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

    private static ParameterCollectionType GetParameterCollectionType(ITypeSymbol parameterTypeSymbol)
    {
        if (parameterTypeSymbol.SpecialType == SpecialType.System_Array)
            return ParameterCollectionType.Array;

        var fullTypeName = parameterTypeSymbol.ToDisplayString();

        if (!fullTypeName.StartsWith("System.Collections.Generic"))
            return ParameterCollectionType.None;

        var typeName = parameterTypeSymbol.Name;
        
        // Best guess on most common types
        if (typeName.StartsWith("List")) return ParameterCollectionType.List;
        if (typeName.StartsWith("IList")) return ParameterCollectionType.Array;
        if (typeName.StartsWith("IEnumerable")) return ParameterCollectionType.Array;
        if (typeName.StartsWith("ICollection")) return ParameterCollectionType.List;
        if (typeName.StartsWith("IReadOnlyList")) return ParameterCollectionType.Array;
        if (typeName.StartsWith("IReadOnlyCollection")) return ParameterCollectionType.List;
        
        // Sets
        if (typeName.StartsWith("HashSet")) return ParameterCollectionType.HashSet;
        if (typeName.StartsWith("ISet")) return ParameterCollectionType.HashSet;
        if (typeName.StartsWith("IReadOnlySet")) return ParameterCollectionType.HashSet;
        if (typeName.StartsWith("SortedSet")) return ParameterCollectionType.SortedSet;
        
        // Other
        if (typeName.StartsWith("Stack")) return ParameterCollectionType.Stack;
        if (typeName.StartsWith("Queue")) return ParameterCollectionType.Queue;
        if (typeName.StartsWith("LinkedList")) return ParameterCollectionType.LinkedList;

        return ParameterCollectionType.None;
    }
}