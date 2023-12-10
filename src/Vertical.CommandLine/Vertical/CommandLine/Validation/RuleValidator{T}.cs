namespace Vertical.CommandLine.Validation;

/// <summary>
/// Represents a validator that is based on a rule and message formatter.
/// </summary>
/// <typeparam name="T">Value type.</typeparam>
public class RuleValidator<T> : IValidator<T>
{
    private readonly Predicate<T> _predicate;

    public RuleValidator(Predicate<T> predicate, Func<ValidationContext<T>, string>? messageFormatter)
    {
        _predicate = predicate;
        MessageFormatter = messageFormatter;
    }
    
    /// <inheritdoc />
    public Type ServiceType => typeof(IValidator<T>);

    /// <inheritdoc />
    public bool Validate(ValidationContext<T> context) => _predicate(context.AttemptedValue);

    /// <inheritdoc />
    public Func<ValidationContext<T>, string>? MessageFormatter { get; }
}