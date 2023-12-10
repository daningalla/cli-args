using Vertical.CommandLine.Utilities;

namespace Vertical.CommandLine.Binding.Pipeline;

#if DEBUG
public class ValidateConfigurationTask : IBindingTask
{
    /// <inheritdoc />
    public void Invoke(IBindingContext context, Action<IBindingContext> next)
    {
        var exceptions = ConfigurationValidator.Validate(context.RootCommand);
        
        switch (exceptions.Count)
        {
            case 1: throw exceptions.First();
            case > 1: throw new AggregateException(exceptions);
        }

        next(context);
    }
}
#endif