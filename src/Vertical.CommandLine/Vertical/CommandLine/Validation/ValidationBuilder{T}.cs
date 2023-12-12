namespace Vertical.CommandLine.Validation;

public sealed class ValidatorBuilder<T>
{
    private readonly List<ValueConstraint<T>> _constraints = new(4);
    
    public ValidatorBuilder<T> Must(Func<T, bool> predicate, Func<T, string>? messageProvider = null)
    {
        _constraints.Add(new ValueConstraint<T>(predicate, messageProvider));
        return this;
    }

    internal IValidator<T> Build() => new MultiConstraintValidator<T>(_constraints);
}