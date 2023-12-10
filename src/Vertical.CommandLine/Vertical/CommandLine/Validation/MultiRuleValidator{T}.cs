namespace Vertical.CommandLine.Validation;

public sealed class MultiRuleValidator<T> : IValidator<T>
{
    private readonly IEnumerable<Predicate<T>> _predicates;

    public MultiRuleValidator(
        IEnumerable<Predicate<T>> predicates,
        Func<ValidationContext<T>, string>? messageFormatter)
    {
        _predicates = predicates;
        MessageFormatter = messageFormatter;
    }
    
    /// <inheritdoc />
    public Type ServiceType => typeof(IValidator<T>);

    /// <inheritdoc />
    public bool Validate(ValidationContext<T> context) => _predicates.All(predicate => predicate(context.AttemptedValue));

    /// <inheritdoc />
    public Func<ValidationContext<T>, string>? MessageFormatter { get; }
}