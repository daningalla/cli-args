using Vertical.CommandLine.Configuration;
using Vertical.CommandLine.Conversion;
using Vertical.CommandLine.Validation;

namespace Vertical.CommandLine.Binding;

internal sealed class ArgumentValueBindingFactory<T> : IArgumentValueBindingFactory
{
    /// <inheritdoc />
    public IArgumentValueBinding CreateBinding(
        CliBindingSymbol bindingSymbol,
        BindingServiceCollection services,
        IEnumerable<string> values)
    {
        var typedSymbol = (CliBindingSymbol<T>)bindingSymbol;

        return CreateBinding(
            typedSymbol,
            typedSymbol.Converter ?? services.GetRequiredService<IValueConverter<T>>(),
            typedSymbol.Validator ?? services.GetService<IValidator<T>>(),
            values);
    }

    private static IArgumentValueBinding CreateBinding(
        CliBindingSymbol<T> symbol,
        IValueConverter<T> converter,
        IValidator<T>? validator,
        IEnumerable<string> values)
    {
        var typedValues = values
            .Select(value =>
            {
                var convertedValue = ConvertValue(symbol, converter, value);
                if (validator != null)
                {
                    ValidateValue(symbol, validator, convertedValue);
                }

                return convertedValue;
            })
            .ToArray();

        if (typedValues.Length > 0 || symbol.Arity.AllowsMany)
        {
            return new ArgumentValueBinding<T>(symbol, typedValues);
        }

        var defaultValue = (symbol.DefaultProvider ?? DefaultValueProvider<T>.Instance).Value;
        return new ArgumentValueBinding<T>(symbol, new[] { defaultValue });
    }

    private static T ConvertValue(CliBindingSymbol<T> symbol, IValueConverter<T> converter, string value)
    {
        try
        {
            return converter.Convert(new ConversionContext<T>(value, symbol));
        }
        catch (Exception exception)
        {
            throw CommandLineException.ConversionFailed(symbol, value, exception);
        }
    }
    
    private static void ValidateValue(CliBindingSymbol<T> symbol, IValidator<T> validator, T bindingValue)
    {
        var context = new ValidationContext<T>(symbol, bindingValue);
        validator.Validate(context);

        if (context.IsValid)
            return;
        
        throw CommandLineException.ValidationFailed(context);
    }
}