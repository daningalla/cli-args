using Vertical.CommandLine.Configuration;

namespace Vertical.CommandLine.Conversion;

public readonly struct ConversionContext<T>
{
    internal ConversionContext(string value, CliBindingSymbol<T> bindingSymbol)
    {
        Value = value;
        BindingSymbol = bindingSymbol;
    }
    
    public string Value { get; }

    public CliBindingSymbol<T> BindingSymbol { get; }
}