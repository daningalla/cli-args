namespace Vertical.CommandLine.Syntax;

/// <summary>
/// Represents an strongly-typed string wrapper.
/// </summary>
public readonly struct IdentifierName : IEquatable<IdentifierName>
{
    private IdentifierName(string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);
        Value = value;
    }

    /// <summary>
    /// Gets the identifier name.
    /// </summary>
    public string Value { get; }

    /// <inheritdoc />
    public override string ToString() => Value;

    /// <inheritdoc />
    public bool Equals(IdentifierName other)
    {
        return Value == other.Value;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is IdentifierName other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(IdentifierName x, IdentifierName y) => x.Equals(y);
    public static bool operator !=(IdentifierName x, IdentifierName y) => !x.Equals(y);
}