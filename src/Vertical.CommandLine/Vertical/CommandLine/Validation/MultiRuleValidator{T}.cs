namespace Vertical.CommandLine.Validation;

public sealed class MultiRuleValidator<T> : IValidator<T>
{
    private readonly IEnumerable<ValidationRuleImplementation<T>> _ruleImplementations;

    public MultiRuleValidator(IEnumerable<ValidationRuleImplementation<T>> ruleImplementations)
    {
        _ruleImplementations = ruleImplementations;
    }
    
    /// <inheritdoc />
    public Type ServiceType => typeof(IValidator<T>);

    /// <inheritdoc />
    public bool Validate(ValidationContext<T> context) => _ruleImplementations.All(impl => impl.Predicate(context));

    /// <inheritdoc />
    public Func<ValidationContext<T>, string>? MessageFormatter { get; }
}