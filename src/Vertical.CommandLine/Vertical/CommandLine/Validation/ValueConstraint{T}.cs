namespace Vertical.CommandLine.Validation;

public class ValueConstraint<T>
{
    private readonly Func<T, bool> _validator;

    /// <summary>
    /// Creates a new instance of this type.
    /// </summary>
    /// <param name="validator">A function that evaluates the binding value and returns a <c>bool</c>
    /// indicating whether or not the value is valid.</param>
    /// <param name="messageFormatter">A function that formats the message to display if validation fails.</param>
    public ValueConstraint(Func<T, bool> validator, Func<T, string>? messageFormatter)
    {
        ArgumentNullException.ThrowIfNull(validator);
        
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