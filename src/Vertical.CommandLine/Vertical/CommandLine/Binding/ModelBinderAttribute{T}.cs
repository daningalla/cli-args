namespace Vertical.CommandLine.Binding;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ModelBinderAttribute<T> : Attribute
{
}