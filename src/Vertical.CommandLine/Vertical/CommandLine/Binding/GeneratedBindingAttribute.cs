namespace Vertical.CommandLine.Binding;

[AttributeUsage(AttributeTargets.Class)]
public class GeneratedBindingAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Class)]
public class GeneratedBindingAttribute<T> : Attribute where T : class
{
}