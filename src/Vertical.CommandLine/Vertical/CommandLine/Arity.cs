namespace Vertical.CommandLine;

/// <summary>
/// Specifies the usage arity of an option or argument.
/// </summary>
public readonly struct Arity : IEquatable<Arity>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Arity"/> structure.
    /// </summary>
    /// <param name="minCount">The minimum number of uses required by an option or argument.</param>
    /// <param name="maxCount">The maximum number of uses allowed by an option or argument.</param>
    /// <exception cref="ArgumentException"><paramref name="minCount"/> is negative.</exception>
    /// <exception cref="ArgumentException"><paramref name="maxCount"/> is less than minCount.</exception>
    public Arity(int minCount, int? maxCount)
    {
        if (minCount < 0)
        {
            throw new ArgumentException("Value must be non-negative.", nameof(minCount));
        }

        if (maxCount < minCount)
        {
            throw new ArgumentException("Value must be equal to or greater than the maximum count.", nameof(maxCount));
        }

        MinCount = minCount;
        MaxCount = maxCount;
    }

    /// <summary>
    /// Gets an <see cref="Arity"/> value that allows no more than a single use.
    /// </summary>
    public static Arity ZeroOrOne => new(0, 1);

    /// <summary>
    /// Gets an <see cref="Arity"/> value that allows multiple uses.
    /// </summary>
    public static Arity ZeroOrMany => new(0, null);

    /// <summary>
    /// Gets an <see cref="Arity"/> value that requires one use.
    /// </summary>
    public static Arity One => new(1, 1);

    /// <summary>
    /// Gets an <see cref="Arity"/> value that requires at least one use.
    /// </summary>
    public static Arity OneOrMany => new(1, null);

    /// <summary>
    /// Gets an <see cref="Arity"/> values that requires a specific number of uses.
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public static Arity Exactly(int count) => new(count, count);

    /// <summary>
    /// Gets the minimum number of uses required for an option or argument.
    /// </summary>
    public int MinCount { get; }

    /// <summary>
    /// Gets the maximum number of uses allowed for an option or argument.
    /// </summary>
    public int? MaxCount { get; }

    /// <summary>
    /// Gets whether the arity has an unconstrained max count.
    /// </summary>
    public bool AllowsMany => MinCount > 1 || MaxCount == null;
    
    /// <inheritdoc />
    public override string ToString() => $"(min={MinCount},max={MaxCount})";

    /// <inheritdoc />
    public bool Equals(Arity other) => MinCount == other.MinCount && MaxCount == other.MaxCount;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Arity other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(MinCount, MaxCount);

    public static bool operator ==(Arity x, Arity y) => x.Equals(y);
    public static bool operator !=(Arity x, Arity y) => !x.Equals(y);
}