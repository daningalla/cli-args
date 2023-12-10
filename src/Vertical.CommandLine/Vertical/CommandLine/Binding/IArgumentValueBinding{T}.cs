using Vertical.CommandLine.Configuration;

namespace Vertical.CommandLine.Binding;

public interface IArgumentValueBinding<T> : IArgumentValueBinding
{
    CliBindingSymbol<T> Symbol { get; }
}