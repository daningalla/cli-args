using Vertical.CommandLine.Binding;

namespace Vertical.CommandLine.Conversion;

/// <summary>
/// Represents a value converter.
/// </summary>
public interface IValueConverter : IBindingService
{
    /// <summary>
    /// Gets the type of value this converter provides.
    /// </summary>
    Type ValueType { get; }
}