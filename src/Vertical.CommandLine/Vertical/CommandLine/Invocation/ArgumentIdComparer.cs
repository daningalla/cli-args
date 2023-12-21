using Vertical.CommandLine.Utilities;

namespace Vertical.CommandLine.Invocation;

public sealed class ArgumentIdComparer : IEqualityComparer<ArgumentId>
{
    /// <inheritdoc />
    public bool Equals(ArgumentId x, ArgumentId y)
    {
        var spanX = x.Id.AsSpan();
        var spanY = y.Id.AsSpan();
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
    public int GetHashCode(ArgumentId obj)
    {
        var id = ReusableStringBuilder.Build(sb =>
        {
            foreach (var chr in obj.Id.Where(char.IsLetterOrDigit))
            {
                sb.Append(char.ToLower(chr));
            }
        });

        return id.GetHashCode();
    }
}