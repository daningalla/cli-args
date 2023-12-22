using CommunityToolkit.Diagnostics;

namespace Vertical.CommandLine.Binding;

/// <summary>
/// Specifies the option, switch, or argument id to bind to.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public sealed class BindingAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance using the specified binding id.
    /// </summary>
    /// <param name="bindingId">
    /// The binding id of the option, argument, or switch.
    /// </param>
    public BindingAttribute(string bindingId)
    {
        Guard.IsNotNullOrWhiteSpace(bindingId);
        BindingId = bindingId;
    }

    public string BindingId { get; }
}