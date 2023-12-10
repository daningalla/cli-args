namespace Vertical.CommandLine.Syntax;

/// <summary>
/// Represents an strongly-typed string wrapper.
/// </summary>
public readonly struct OperandValue : IEquatable<OperandValue>
{
    private OperandValue(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        Value = value;
    }

    /// <summary>
    /// Gets the identifier name.
    /// </summary>
    public string Value { get; }

    /// <inheritdoc />
    public override string ToString() => Value;

    /// <inheritdoc />
    public bool Equals(OperandValue other) => Value == other.Value;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is OperandValue other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(OperandValue x, OperandValue y) => x.Equals(y);
    public static bool operator !=(OperandValue x, OperandValue y) => !x.Equals(y);
}