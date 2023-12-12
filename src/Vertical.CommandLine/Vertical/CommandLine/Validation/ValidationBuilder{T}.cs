namespace Vertical.CommandLine.Validation;

internal sealed class ValidatorBuilder<T> : IValidatorBuilder<T>
{
    private readonly List<ValueConstraint<T>> _constraints = new(4);
    
    /// <inheritdoc />
    public IValidatorBuilder<T> Must(Func<T, bool> predicate, Func<T, string>? messageProvider)
    {
        _constraints.Add(new ValueConstraint<T>(predicate, messageProvider));
        return this;
    }

    internal IValidator<T> Build() => new MultiConstraintValidator<T>(_constraints);
}