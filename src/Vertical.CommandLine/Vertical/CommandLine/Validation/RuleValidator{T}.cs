namespace Vertical.CommandLine.Validation;

/// <summary>
/// Represents a validator that is based on a rule and message formatter.
/// </summary>
/// <typeparam name="T">Value type.</typeparam>
public class RuleValidator<T> : IValidator<T>
{
    private readonly ValidationRuleImplementation<T> _implementation;

    public RuleValidator(ValidationRuleImplementation<T> implementation)
    {
        _implementation = implementation;
    }
    
    /// <inheritdoc />
    public Type ServiceType => typeof(IValidator<T>);

    /// <inheritdoc />
    public bool Validate(ValidationContext<T> context) => _implementation.Predicate(context);

    /// <inheritdoc />
    public Func<ValidationContext<T>, string>? MessageFormatter => _implementation.MessageProvider;
}