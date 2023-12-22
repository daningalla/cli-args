using Vertical.CommandLine.Configuration;

namespace Vertical.CommandLine.Conversion;

/// <summary>
/// Represents data used in value conversions.
/// </summary>
/// <typeparam name="T">Target type of the conversion.</typeparam>
public readonly struct ConversionContext<T>
{
    internal ConversionContext(string value, CliBindingSymbol<T> bindingSymbol)
    {
        Value = value;
        BindingSymbol = bindingSymbol;
    }
    
    /// <summary>
    /// Gets the string CLI value to convert.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Gets the binding symbol that is requesting the conversion.
    /// </summary>
    public CliBindingSymbol<T> BindingSymbol { get; }
}