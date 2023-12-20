using Microsoft.CodeAnalysis;

namespace Vertical.CommandLine;

public class PropertyMetadata : IMemberMetadata
{
    public PropertyMetadata(IPropertySymbol propertySymbol)
    {
        Name = propertySymbol.Name;
        Type = propertySymbol.Type;
        BindingId = Utilities.GetExplicitSymbolBindingId(propertySymbol) ?? Utilities.EnsureCamelCase(Name);
        CollectionType = Utilities.GetSymbolCollectionType(propertySymbol.Type);
        CollectionElementType = propertySymbol.Type.GetGenericArgumentType();
    }

    /// <inheritdoc />
    public CollectionType CollectionType { get; }

    /// <inheritdoc />
    public ITypeSymbol? CollectionElementType { get; }

    /// <inheritdoc />
    public string BindingId { get; }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public ITypeSymbol Type { get; }
}