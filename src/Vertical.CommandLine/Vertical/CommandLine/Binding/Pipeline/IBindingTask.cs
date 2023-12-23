namespace Vertical.CommandLine.Binding.Pipeline;

internal interface IBindingTask
{
    void Invoke(IBindingContext context, Action<IBindingContext> next);
}