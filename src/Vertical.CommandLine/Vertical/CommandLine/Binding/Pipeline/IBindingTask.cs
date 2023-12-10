namespace Vertical.CommandLine.Binding.Pipeline;

public interface IBindingTask
{
    void Invoke(IBindingContext context, Action<IBindingContext> next);
}