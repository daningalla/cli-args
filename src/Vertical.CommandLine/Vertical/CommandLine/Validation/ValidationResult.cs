namespace Vertical.CommandLine.Validation;

public readonly struct ValidationResult
{
    private ValidationResult(bool isValid, string? message)
    {
        IsValid = isValid;
        Message = message ?? string.Empty;
    }

    /// <summary>
    /// Gets whether the value was valid.
    /// </summary>
    public bool IsValid { get; }

    /// <summary>
    /// Gets the message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets an instance of this type that indicates the value was valid.
    /// </summary>
    /// <returns><see cref="ValidationResult"/></returns>
    public static ValidationResult Success => new(true, null);

    /// <summary>
    /// Gets an instance of this type that indicates the value was not valid.
    /// </summary>
    /// <param name="message">Message to display that describes the invalid state.</param>
    /// <returns><see cref="ValidationResult"/></returns>
    public static ValidationResult Fail(string message) => new(false, message);
}