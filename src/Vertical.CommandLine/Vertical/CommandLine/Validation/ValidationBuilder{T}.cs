namespace Vertical.CommandLine.Validation;

internal sealed class ValidationBuilder<T> : IValidationBuilder<T>
{
    private readonly List<ValueConstraint<T>> _constraints = new(4);
    
    /// <inheritdoc />
    public IValidationBuilder<T> Must(Func<T, bool> predicate, Func<T, string>? messageProvider)
    {
        _constraints.Add(new ValueConstraint<T>(predicate, messageProvider));
        return this;
    }

    internal static IValidator<T>? Configure(Action<IValidationBuilder<T>>? configure) 
    {
        if (configure == null)
            return null;

        var builder = new ValidationBuilder<T>();
        configure(builder);

        return new Validator<T>(builder._constraints);
    }
}