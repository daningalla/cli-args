using Vertical.CommandLine.Configuration;

namespace Vertical.CommandLine.Binding;

public interface IArgumentValueBindingFactory
{
    /// <summary>
    /// When implemented by a class, creates a value binding.
    /// </summary>
    /// <param name="bindingSymbol">The symbol being bound.</param>
    /// <param name="services">Binding services.</param>
    /// <param name="values">The values being bound.</param>
    /// <returns><see cref="IArgumentValueBinding"/></returns>
    IArgumentValueBinding CreateBinding(
        CliBindingSymbol bindingSymbol,
        BindingServiceCollection services,
        IEnumerable<string> values);
}