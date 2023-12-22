using Vertical.CommandLine.Utilities;

namespace Vertical.CommandLine.Binding;

internal sealed class BindingNameComparer : IEqualityComparer<string>
{
    internal static IEqualityComparer<string> Instance { get; } = new BindingNameComparer();
    
    /// <inheritdoc />
    public bool Equals(string? x, string? y)
    {
        if (ReferenceEquals(x, null) && ReferenceEquals(y, null))
            return true;

        if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            return false;
        
        var spanX = x.AsSpan();
        var spanY = y.AsSpan();
        var last = (x: char.MinValue, y: char.MinValue);

        while (spanX.Length > 0 && spanY.Length > 0)
        {
            var current = (x: spanX[0], y: spanY[0]);

            switch (current)
            {
                case { } when current.x == current.y:
                case { } when char.ToLower(current.x) == char.ToLower(current.y) &&
                              (last.x is '-' or '/' || last.y is '-' or '/'):
                    spanX = spanX[1..];
                    spanY = spanY[1..];
                    break;

                case { x: '-' or '/' }:
                    spanX = spanX[1..];
                    break;

                case { y: '-' or '/' }:
                    spanY = spanY[1..];
                    break;
                
                default:
                    return false;
            }

            last = current;
        }

        return true;
    }

    /// <inheritdoc />
    public int GetHashCode(string value)
    {
        var id = ReusableStringBuilder.Build(sb =>
        {
            foreach (var chr in value.Where(char.IsLetterOrDigit))
            {
                sb.Append(char.ToLower(chr));
            }
        });

        return id.GetHashCode();
    }
}