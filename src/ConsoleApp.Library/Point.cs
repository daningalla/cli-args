namespace ConsoleApp.Library;

public readonly record struct Point(int X, int Y) : IParsable<Point>
{
    /// <inheritdoc />
    public static Point Parse(string s, IFormatProvider? provider)
    {
        var split = s.Split(',');
        return new Point(int.Parse(split[0]), int.Parse(split[1]));
    }

    /// <inheritdoc />
    public static bool TryParse(string? s, IFormatProvider? provider, out Point result)
    {
        result = s != null
            ? Parse(s, provider)
            : default;

        return s != null;
    }
}