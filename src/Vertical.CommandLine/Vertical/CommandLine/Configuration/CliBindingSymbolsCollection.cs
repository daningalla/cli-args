using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Vertical.CommandLine.Configuration;

public sealed class CliBindingSymbolsCollection : IEnumerable<CliBindingSymbol>
{
    private readonly Queue<CliBindingSymbol> _optionQueue;
    private readonly Queue<CliBindingSymbol> _argumentQueue;
    
    internal CliBindingSymbolsCollection(IReadOnlyCollection<CliBindingSymbol> symbols)
    {
        _optionQueue = new Queue<CliBindingSymbol>(symbols.Where(symbol => symbol.SymbolType != CliSymbolType.Argument));
        _argumentQueue = new Queue<CliBindingSymbol>(symbols.Where(symbol => symbol.SymbolType == CliSymbolType.Argument));
    }

    /// <inheritdoc />
    public IEnumerator<CliBindingSymbol> GetEnumerator() => _optionQueue
        .Concat(_argumentQueue)
        .GetEnumerator();

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Attempts to remove the next option binding symbol.
    /// </summary>
    /// <param name="symbol">If the method is successful, the binding option symbol.</param>
    /// <returns><c>true</c> if the binding was removed.</returns>
    public bool TryRemoveOptionBinding([NotNullWhen(true)] out CliBindingSymbol? symbol)
    {
        return _optionQueue.TryDequeue(out symbol);
    }

    /// <summary>
    /// Attempts to remove the next argument binding symbol.
    /// </summary>
    /// <param name="symbol">If the method is successful, the binding argument symbol.</param>
    /// <returns><c>true</c> if the binding was removed.</returns>
    public bool TryRemoveArgumentBinding([NotNullWhen(true)] out CliBindingSymbol? symbol)
    {
        return _argumentQueue.TryDequeue(out symbol);
    }
}