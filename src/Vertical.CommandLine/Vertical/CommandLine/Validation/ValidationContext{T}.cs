using Vertical.CommandLine.Configuration;

namespace Vertical.CommandLine.Validation;

public readonly struct ValidationContext<T>
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
}