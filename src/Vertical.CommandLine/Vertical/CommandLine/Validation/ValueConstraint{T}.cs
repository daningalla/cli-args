namespace Vertical.CommandLine.Validation;

public class ValueConstraint<T>
{
    private readonly Func<T, bool> _validator;

    public ValueConstraint(Func<T, bool> validator, Func<T, string>? messageFormatter)
    {
        MessageFormatter = messageFormatter;
        _validator = validator;
    }

    /// <summary>
    /// Gets if the specified value is valid.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <returns><c>bool</c></returns>
    public bool IsValid(T value) => _validator(value);

    /// <summary>
    /// Gets an object that formats the error message.
    /// </summary>
    public Func<T, string>? MessageFormatter { get; }
}