﻿using Vertical.CommandLine.Configuration;
using Vertical.CommandLine.Syntax;
using Vertical.CommandLine.Utilities;
using Vertical.CommandLine.Validation;

namespace Vertical.CommandLine;

public class CommandLineException : Exception
{
    private const string SymbolKey = "Symbol";
    private const string AttemptedValueKey = "AttemptedValue";
    private const string AttemptedValuesKey = "AttemptedValues";
    private const string ValidatorKey = "Validator";

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandLineException"/> class.
    /// </summary>
    /// <param name="error">Error code.</param>
    /// <param name="message">Message that describes the exception.</param>
    /// <param name="innerException">The exception that causes this instance to be thrown.</param>
    public CommandLineException(
        CommandLineError error,
        string message, 
        Exception? innerException = null) : base(message, innerException)
    {
        Error = error;
    }

    /// <summary>
    /// Gets the associated binding symbol or <c>null</c>.
    /// </summary>
    public CliSymbol? BindingSymbol => (CliSymbol?)Data[SymbolKey];

    /// <summary>
    /// Gets the attempted value or <c>null</c>.
    /// </summary>
    public object? AttemptedValue => Data[AttemptedValueKey];

    /// <summary>
    /// Gets the attempted values or <c>null</c>.
    /// </summary>
    public string?[]? AttemptedValues => (string?[]?)Data[AttemptedValuesKey];

    /// <summary>
    /// Gets the validator or <c>null</c>.
    /// </summary>
    public IValidator? Validator => (IValidator?)Data[ValidatorKey];

    /// <summary>
    /// Gets the command line error.
    /// </summary>
    public CommandLineError Error { get; }

    private static Exception Create(
        CommandLineError error,
        string message,
        Exception? innerException = null,
        params (object key, object value)[] data)
    {
        var exception = new CommandLineException(error, message, innerException);
        foreach (var (key, value) in data)
        {
            exception.Data[key] = value;
        }

        return exception;
    }

    internal static Exception ConversionFailed<T>(
        CliBindingSymbol<T> symbol,
        string argumentValue,
        Exception innerException)
    {
        var message = $"{symbol.SymbolType} '{symbol}': Cannot convert argument \"{argumentValue}\" to {typeof(T)}.";
        
        return Create(
            CommandLineError.ConversionFailed,
            message,
            innerException,
            (SymbolKey, symbol),
            (AttemptedValueKey, argumentValue));
    }

    internal static Exception ValidationFailed<T>(
        CliBindingSymbol<T> symbol,
        IValidator<T> validator,
        T attemptedValue,
        Exception exception)
    {
        var message = validator.MessageFormatter?.Invoke(new ValidationContext<T>(symbol, attemptedValue))
                      ?? $"{symbol.SymbolType} '{symbol}': Value \"{attemptedValue}\" is not valid.";

        return Create(
            CommandLineError.ValidationFailed,
            message,
            exception,
            (SymbolKey, symbol),
            (AttemptedValueKey, attemptedValue!),
            (ValidatorKey, validator));
    }

    internal static Exception MinimumArityNotMet(CliBindingSymbol symbol, IEnumerable<string?> arguments)
    {
        var argumentArray = arguments.ToArray();
        var message = ReusableStringBuilder.Build(sb =>
        {
            sb.Append($"{symbol.SymbolType} '{symbol}' expected at least {symbol.Arity.MinCount} arguments, ");
            if (argumentArray.Length == 0)
            {
                sb.AppendLine("but none were received.");
                return;
            }

            sb.AppendLine("but only received the following:");
            foreach (var arg in argumentArray)
            {
                sb.AppendLine($"\t\"{arg}\"");
            }
        });

        return Create(
            CommandLineError.MinimumArityNotMet,
            message,
            null,
            (SymbolKey, symbol),
            (AttemptedValuesKey, argumentArray));
    }

    internal static Exception MaximumArityExceeded(CliBindingSymbol symbol, IEnumerable<string?> arguments)
    {
        var argumentArray = arguments.ToArray();
        var message = ReusableStringBuilder.Build(sb =>
        {
            sb.Append($"{symbol.SymbolType} '{symbol}' expected no more than {symbol.Arity.MaxCount} use, ");

            if (symbol.SymbolType == CliSymbolType.Switch)
            {
                sb.AppendLine($"but {argumentArray.Length} were received.");
                return;
            }
            
            sb.AppendLine("but received the following:");
            foreach (var arg in argumentArray)
            {
                sb.AppendLine($"\t\"{arg}\"");
            }
        });

        return Create(
            CommandLineError.MaximumArityExceeded,
            message,
            null,
            (SymbolKey, symbol),
            (AttemptedValuesKey, argumentArray));
    }

    internal static Exception InvalidArgument(SemanticArgument argument)
    {
        var message = $"Invalid argument \"{argument.Text}\".";

        return Create(
            CommandLineError.InvalidArgument,
            message,
            null,
            (AttemptedValueKey, argument.Text));
    }

    internal static Exception OptionMissingOperand(CliBindingSymbol symbol)
    {
        var message = $"{symbol.SymbolType} '{symbol}' requires an operand value which was not provided.";

        return Create(
            CommandLineError.MissingOperand,
            message,
            null,
            (SymbolKey, symbol));
    }
}