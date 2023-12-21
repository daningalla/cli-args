using System.Diagnostics.CodeAnalysis;
using Vertical.CommandLine.Configuration;

namespace Vertical.CommandLine.Validation;

/// <summary>
/// Represents a context that manages a validation operation.
/// </summary>
/// <typeparam name="T">Value type.</typeparam>
public sealed class ValidationContext<T> : IValidationContext<T>
{
    internal ValidationContext(
        CliBindingSymbol<T> symbol,
        T attemptedValue)
    {
        Symbol = symbol;
        AttemptedValue = attemptedValue;
    }
    
    /// <summary>
    /// Gets the symbol associated with the failed validation.
    /// </summary>
    public CliBindingSymbol<T> Symbol { get; }
    
    /// <summary>
    /// Gets the value that was attempted.
    /// </summary>
    public T AttemptedValue { get; }

    /// <inheritdoc />
    public ICollection<ValidationRule<T>> Failures { get; } = new List<ValidationRule<T>>();

    /// <inheritdoc />
    public bool IsValid => Failures.Count == 0;

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override string ToString() => $"Failures={Failures.Count}";
}