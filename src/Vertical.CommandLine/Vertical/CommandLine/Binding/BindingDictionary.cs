using System.Diagnostics.CodeAnalysis;
using Vertical.CommandLine.Configuration;
using Vertical.CommandLine.Utilities;

namespace Vertical.CommandLine.Binding;

public sealed class BindingDictionary<T>
{
    private readonly IReadOnlyDictionary<string, T> _dictionary;

    private BindingDictionary(IReadOnlyDictionary<string, T> dictionary) => _dictionary = dictionary;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="BindingDictionary{T}"/> class.
    /// </summary>
    /// <param name="source">Source items.</param>
    /// <param name="symbolSelector">A function that returns the associated <see cref="CliBindingSymbol"/></param>
    /// <returns><see cref="BindingDictionary{T}"/></returns>
    public static BindingDictionary<T> Create(
        IEnumerable<T> source,
        Func<T, CliSymbol> symbolSelector)
    {
        var sourceArray = source.ToArray();
        var dictionary = new Dictionary<string, T>(sourceArray.Length * 2);

        foreach (var item in sourceArray)
        {
            var symbol = symbolSelector(item);
            dictionary[symbol.Id] = item;
            dictionary[NamingUtilities.GetInferredBindingName(symbol.Id)] = item;
        }

        return new BindingDictionary<T>(dictionary);
    }

    /// <summary>
    /// Gets whether the given key is present in the collection.
    /// </summary>
    /// <param name="key">Key.</param>
    /// <returns><c>bool</c></returns>
    public bool ContainsKey(string key) => _dictionary.ContainsKey(key);

    /// <summary>
    /// Gets the item with the specified key.
    /// </summary>
    /// <param name="key">Key value.</param>
    public T this[string key] => _dictionary[key];

    /// <summary>
    /// Gets the keys in the dictionary.
    /// </summary>
    public IEnumerable<string> Keys => _dictionary.Keys;

    /// <summary>
    /// Gets the values in the dictionary.
    /// </summary>
    public IEnumerable<T> Values => _dictionary.Values;

    /// <summary>
    /// Gets the item with the specified key.
    /// </summary>
    /// <param name="key">Item key.</param>
    /// <param name="binding">If located, the dictionary entry.</param>
    /// <returns><c>true</c> if <paramref name="key"/> was found.</returns>
    public bool TryGetValue(string key, [NotNullWhen(true)] out T? binding) =>
        _dictionary.TryGetValue(key, out binding!);
}