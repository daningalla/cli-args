using Vertical.CommandLine.Configuration;

namespace Vertical.CommandLine.Binding;

/// <summary>
/// Represents the base interface of a value binding.
/// </summary>
public interface IArgumentValueBinding
{
    /// <summary>
    /// Gets the primary binding id.
    /// </summary>
    string BindingId { get; }
    
    /// <summary>
    /// Gets the value type managed by this binding.
    /// </summary>
    Type ValueType { get; }
    
    /// <summary>
    /// Gets the base CLI symbol.
    /// </summary>
    CliSymbol BaseSymbol { get; }
}

