namespace Vertical.CommandLine.Utilities;

/// <summary>
/// Utilities concerned with naming conventions.
/// </summary>
internal static class NamingUtilities
{
    internal static string GetInferredBindingName(string id)
    {
        using var stringBuilderRef = ReusableStringBuilder.GetInstance();
        var sb = stringBuilderRef.Value;
        var c = 0;
        
        // Strip leading prefixes
        for (; c < id.Length && id[c] is '-' or '/'; c++)
        {
        }
        
        // Ensure next character is lower case
        if (c < id.Length)
        {
            sb.Append(char.ToLower(id[c]));
            c++;
        }

        var nextCharIsUpperCase = false;
        for (; c < id.Length; c++)
        {
            var chr = id[c];
            if (chr == '-')
            {
                nextCharIsUpperCase = true;
                continue;
            }

            chr = nextCharIsUpperCase ? char.ToUpper(chr) : chr;
            sb.Append(chr);
            nextCharIsUpperCase = false;
        }

        var name = sb.ToString();
        return name;
    }

    internal static string ToCamelCase(string str)
    {
        return str.Length switch
        {
            1 when char.IsUpper(str[0]) => char.ToLower(str[0]).ToString(),
            > 1 when char.IsUpper(str[0]) => $"{char.ToLower(str[0])}{str[1..]}",
            _ => str
        };
    }
}