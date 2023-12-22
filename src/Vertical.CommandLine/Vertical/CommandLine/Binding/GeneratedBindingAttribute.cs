namespace Vertical.CommandLine.Binding;

/// <summary>
/// Marks a class as a model that is used in binder generation.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class GeneratedBindingAttribute : Attribute
{
}

/// <summary>
/// Marks a class as being a generated model binder.
/// </summary>
/// <typeparam name="T">Model type being bound.</typeparam>
[AttributeUsage(AttributeTargets.Class)]
public class GeneratedBindingAttribute<T> : Attribute where T : class
{
}