using Microsoft.CodeAnalysis;

namespace Vertical.CommandLine;

public sealed class ParameterMetadata : IMemberMetadata
{
    public ParameterMetadata(IParameterSymbol parameterSymbol)
    {
        Name = parameterSymbol.Name;
        Type = parameterSymbol.Type;
        BindingId = Utilities.GetExplicitSymbolBindingId(parameterSymbol) ?? Name;
        CollectionType = Utilities.GetSymbolCollectionType(parameterSymbol.Type);
        CollectionElementType = parameterSymbol.Type.GetGenericArgumentType();
        Ordinal = parameterSymbol.Ordinal;
        IsCancellationToken = parameterSymbol.Type.ToDisplayString() ==
                              "System.Threading.CancellationToken";
    }

    public bool IsCancellationToken { get; set; }

    public CollectionType CollectionType { get; set; }

    public ITypeSymbol? CollectionElementType { get; set; }

    public int Ordinal { get; set; }

    public string BindingId { get; }

    public string Name { get; }

    public ITypeSymbol Type { get; }
}