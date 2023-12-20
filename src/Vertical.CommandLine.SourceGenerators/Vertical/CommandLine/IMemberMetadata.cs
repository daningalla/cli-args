using Microsoft.CodeAnalysis;

namespace Vertical.CommandLine;

public interface IMemberMetadata
{
    public CollectionType CollectionType { get; }
    
    public ITypeSymbol? CollectionElementType { get; }
    
    public string BindingId { get; }
    
    public string Name { get; }
    
    public ITypeSymbol Type { get; }
}