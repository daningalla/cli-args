using Microsoft.CodeAnalysis;

namespace Vertical.CommandLine;

public sealed class GeneratedBindingMetadata
{
    private sealed class ComparerImpl : IEqualityComparer<GeneratedBindingMetadata?>
    {
        /// <inheritdoc />
        public bool Equals(GeneratedBindingMetadata? x, GeneratedBindingMetadata? y)
        {
            if (ReferenceEquals(x, null) && ReferenceEquals(y, null))
                return true;

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;

            return x.BinderTypeNamespace == y.BinderTypeNamespace &&
                   x.BinderTypeName == y.BinderTypeName &&
                   SymbolEqualityComparer.Default.Equals(x.ModelTypeSymbol, y.ModelTypeSymbol);
        }

        /// <inheritdoc />
        public int GetHashCode(GeneratedBindingMetadata? obj)
        {
            return obj != null
                ? HashCode.Combine(
                    obj.BinderTypeNamespace, 
                    obj.BinderTypeName, 
                    SymbolEqualityComparer.Default.GetHashCode(obj.ModelTypeSymbol))
                : 0;
        }
    }

    public static readonly IEqualityComparer<GeneratedBindingMetadata?> Comparer = new ComparerImpl();
    
    public GeneratedBindingMetadata(
        string? binderTypeNamespace,
        string binderTypeName,
        ISymbol modelTypeSymbol)
    {
        BinderTypeNamespace = binderTypeNamespace;
        BinderTypeName = binderTypeName;
        ModelTypeSymbol = modelTypeSymbol;
    }

    public string? BinderTypeNamespace { get; }
    public string BinderTypeName { get; }
    public ISymbol ModelTypeSymbol { get; }
}    