using Vertical.CommandLine.Binding;
using Vertical.CommandLine.Configuration;
using Vertical.CommandLine.Conversion;
using Vertical.CommandLine.Validation;

namespace Vertical.CommandLine.Invocation;

internal sealed class MappedArgumentProvider : IMappedArgumentProvider
{
    internal MappedArgumentProvider(
        BindingServiceCollection services,
        IEnumerable<IArgumentValueBinding> bindings)
    {
        Services = services;
        Bindings = BindingDictionary<IArgumentValueBinding>.Create(bindings, item => item.BaseSymbol);
    }

    /// <summary>
    /// Gets the binding dictionary keyed by parameter name.
    /// </summary>
    public BindingDictionary<IArgumentValueBinding> Bindings { get; }

    /// <summary>
    /// Gets binding services.
    /// </summary>
    public BindingServiceCollection Services { get; }

    /// <inheritdoc />
    public T GetValue<T>(string parameterId)
    {
        var binding = (SingleArgumentValueBinding<T>)Bindings[parameterId];
        var symbol = binding.Symbol;

        if (binding.ArgumentValue == null)
        {
            var defaultProvider = symbol.DefaultProvider ?? DefaultValueProvider<T>.Instance;
            return defaultProvider.Value;
        }

        var converter = ResolveConverterOrThrow(symbol);
        var candidateValue = ConvertValue(symbol, converter, binding.ArgumentValue);

        var validator = symbol.Validator ?? Services.GetService<IValidator<T>>();
        if (validator != null)
        {
            ValidateValue(symbol, validator, candidateValue);
        }

        return candidateValue;
    }

    private IValueConverter<T> ResolveConverterOrThrow<T>(CliBindingSymbol<T> symbol)
    {
        return symbol.Converter
               ?? Services.GetService<IValueConverter<T>>()
               ?? DefaultValueConverter<T>.Instance
               ?? throw new InvalidOperationException();
    }

    /// <inheritdoc />
    public T[] GetValueArray<T>(string parameterId) => GetValues<T>(parameterId).ToArray();

    /// <inheritdoc />
    public List<T> GetValueList<T>(string parameterId) => GetValues<T>(parameterId).ToList();

    /// <inheritdoc />
    public LinkedList<T> GetValueLinkedList<T>(string parameterId) => new(GetValues<T>(parameterId));

    /// <inheritdoc />
    public HashSet<T> GetValueHashSet<T>(string parameterId) => new(GetValues<T>(parameterId));

    /// <inheritdoc />
    public SortedSet<T> GetValueSortedSet<T>(string parameterId) => new(GetValues<T>(parameterId));

    /// <inheritdoc />
    public Stack<T> GetValueStack<T>(string parameterId) => new(GetValues<T>(parameterId));

    /// <inheritdoc />
    public Queue<T> GetValueQueue<T>(string parameterId) => new(GetValues<T>(parameterId));

    private static void ValidateValue<T>(CliBindingSymbol<T> symbol, IValidator<T> validator, T bindingValue)
    {
        var context = new ValidationContext<T>(symbol, bindingValue);
        validator.Validate(context);

        if (context.IsValid)
            return;
        
        throw CommandLineException.ValidationFailed(context);
    }

    private static T ConvertValue<T>(CliBindingSymbol<T> symbol, IValueConverter<T> converter, string argumentValue)
    {
        try
        {
            return converter.Convert(new ConversionContext<T>(argumentValue, symbol));
        }
        catch (Exception exception)
        {
            throw CommandLineException.ConversionFailed(symbol, argumentValue, exception);
        }
    }

    private IEnumerable<T> GetValues<T>(string parameterId)
    {
        var binding = (MultiValueArgumentBinding<T>)Bindings[parameterId];
        var symbol = binding.Symbol;
        var converter = ResolveConverterOrThrow(symbol);
        var validator = symbol.Validator ?? Services.GetService<IValidator<T>>();

        foreach (var argumentValue in binding.ArgumentValues)
        {
            var candidateValue = ConvertValue(symbol, converter, argumentValue);
            if (validator != null)
            {
                ValidateValue(symbol, validator, candidateValue);
            }

            yield return candidateValue;
        }
    }
}