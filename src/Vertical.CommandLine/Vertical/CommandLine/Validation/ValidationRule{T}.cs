namespace Vertical.CommandLine.Validation;

public sealed class ValidationRule<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationRule{T}"/> class.
    /// </summary>
    /// <param name="validator"></param>
    /// <param name="messageFormatter"></param>
    public ValidationRule(
        Func<IValidationContext<T>, bool> validator,
        Func<T, string>? messageFormatter = null)
    {
        Validator = validator;
        MessageFormatter = messageFormatter;
    }

    /// <summary>
    /// Gets the function responsible for validation.
    /// </summary>
    public Func<IValidationContext<T>, bool> Validator { get; }

    /// <summary>
    /// Gets a function that formats the message to display when validation fails.
    /// </summary>
    public Func<T, string>? MessageFormatter { get; }
}