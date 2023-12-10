using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Vertical.CommandLine.Configuration;

namespace Vertical.CommandLine.Syntax;

/// <summary>
/// Represents an indexed collection of <see cref="SemanticArgument"/> instances.
/// </summary>
public sealed class SemanticArgumentCollection : IEnumerable<SemanticArgument>
{
    private readonly Dictionary<int, SemanticArgument> _argumentOrdinalDictionary;
    private readonly HashSet<SemanticArgument> _unusedOptionHashSet;
    private readonly ILookup<string, SemanticArgument> _optionIdentifierLookup;

    internal SemanticArgumentCollection(IReadOnlyCollection<SemanticArgument> arguments)
    {
        _argumentOrdinalDictionary = arguments
            .Where(arg => !arg.IsOption)
            .ToDictionary(arg => arg.Ordinal);
        _unusedOptionHashSet = new HashSet<SemanticArgument>(arguments.Where(arg => arg.IsOption));
        _optionIdentifierLookup = _unusedOptionHashSet.ToLookup(arg => arg
            .Anatomy
            .PrefixedIdentifier);
    }

    private IEnumerable<SemanticArgument> UnmappedArguments => _unusedOptionHashSet
        .Concat(_argumentOrdinalDictionary.Values);

    /// <summary>
    /// Gets whether they are arguments remaining in the collection.
    /// </summary>
    public bool IsEmpty => _argumentOrdinalDictionary.Count == 0 && _unusedOptionHashSet.Count == 0;

    /// <summary>
    /// Attempts to access the argument at the specified index.
    /// </summary>
    /// <param name="ordinal">The ordinal index of the argument.</param>
    /// <param name="argument">If the method is successful, the argument.</param>
    /// <returns><c>true</c> if an argument was mapped.</returns>
    public bool TryPeekValueArgument(int ordinal, [NotNullWhen(true)] out SemanticArgument? argument)
    {
        return _argumentOrdinalDictionary.TryGetValue(ordinal, out argument);
    }

    /// <summary>
    /// Removes an argument from the collection.
    /// </summary>
    /// <param name="argument">The argument to remove.</param>
    public void RemoveArgument(SemanticArgument argument)
    {
        _argumentOrdinalDictionary.Remove(argument.Ordinal);
    }

    /// <summary>
    /// Returns <see cref="SemanticArgument"/> instances that match any identifiers of the specified
    /// option symbol.
    /// </summary>
    /// <param name="option">Option to match.</param>
    /// <returns>
    /// A collection of tuples where each contains the matched <see cref="SemanticArgument"/>, and optionally
    /// a value based <see cref="SemanticArgument"/> that is a speculative operand value.
    /// </returns>
    public IEnumerable<(SemanticArgument MatchedArgument, SemanticArgument? SpeculativeOperand)>
        RemoveOptionArguments(CliBindingSymbol option)
    {
        var entries = option
            .Identifiers
            .SelectMany(identifier => _optionIdentifierLookup[identifier])
            .Where(_unusedOptionHashSet.Remove);

        foreach (var entry in entries)
        {
            TryPeekValueArgument(entry.Ordinal + 1, out var operandArgument);
            yield return (entry, operandArgument);
        }
    }

    private IEnumerable<SemanticArgument> GetOptionArguments(CliSymbol option)
    {
        return option
            .Identifiers
            .SelectMany(identifier => _optionIdentifierLookup[identifier])
            .Where(_unusedOptionHashSet.Contains);
    }

    /// <inheritdoc />
    public IEnumerator<SemanticArgument> GetEnumerator() => UnmappedArguments.GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}