using System.Text.RegularExpressions;

namespace Vertical.CommandLine.Validation;

public static class ValidationBuilderStringExtensions
{
    /// <summary>
    /// Adds a rule that passes validation if the string version of the value meets or exceeds the specified length.
    /// </summary>
    /// <param name="builder">Builder</param>
    /// <param name="length">Inclusive minimum length.</param>
    /// <param name="messageFormatter">A function that formats the message to display when validation fails.</param>
    /// <returns>A reference to this instance.</returns>
    public static ValidatorBuilder<T> MinimumLength<T>(
        this ValidatorBuilder<T> builder,
        int length,
        Func<T, string>? messageFormatter = null)
    {
        if (length < 0)
        {
            throw new ArgumentException("Value must be a positive number.", nameof(length));
        }
        
        return builder.MustSatisfy(new ValidationRule<T>(context =>
                context.AttemptedValue is string str && str.Length >= length,
            messageFormatter ?? (value => $"Value \"{value}\" does not meet minimum length of {length}.")));
    }
    
    /// <summary>
    /// Adds a rule that passes validation if the string version of the value is less than or equal to the specified
    /// length.
    /// </summary>
    /// <param name="builder">Builder</param>
    /// <param name="length">Inclusive maximum length.</param>
    /// <param name="messageFormatter">A function that formats the message to display when validation fails.</param>
    /// <returns>A reference to this instance.</returns>
    public static ValidatorBuilder<T> MaximumLength<T>(
        this ValidatorBuilder<T> builder,
        int length,
        Func<T, string>? messageFormatter = null)
    {
        if (length < 0)
        {
            throw new ArgumentException("Value must be a positive number.", nameof(length));
        }
        
        return builder.MustSatisfy(new ValidationRule<T>(context =>
                context.AttemptedValue is string str && str.Length <= length,
            messageFormatter ?? (value => $"Value \"{value}\" exceeds maximum length of {length}.")));
    }
    
    /// <summary>
    /// Adds a rule that passes validation if the string version of the value matches the specified regular expression
    /// pattern.
    /// </summary>
    /// <param name="builder">Builder</param>
    /// <param name="pattern">The regular expression pattern to match.</param>
    /// <param name="messageFormatter">A function that formats the message to display when validation fails.</param>
    /// <returns>A reference to this instance.</returns>
    public static ValidatorBuilder<T> Matches<T>(
        this ValidatorBuilder<T> builder,
        string pattern,
        Func<T, string>? messageFormatter = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(pattern);
        
        return builder.MustSatisfy(new ValidationRule<T>(context =>
            context.AttemptedValue is string str && Regex.IsMatch(str, pattern),
            messageFormatter ?? (value => $"Value \"{value}\" does not match regular expression pattern {pattern}.")));
    }
    
    /// <summary>
    /// Adds a rule that passes validation if the string version of the value matches the specified regular expression.
    /// </summary>
    /// <param name="builder">Builder</param>
    /// <param name="regex">The regular expression to match.</param>
    /// <param name="messageFormatter">A function that formats the message to display when validation fails.</param>
    /// <returns>A reference to this instance.</returns>
    public static ValidatorBuilder<T> Matches<T>(
        this ValidatorBuilder<T> builder,
        Regex regex,
        Func<T, string>? messageFormatter = null)
    {
        ArgumentNullException.ThrowIfNull(regex);
        
        return builder.MustSatisfy(new ValidationRule<T>(context =>
            context.AttemptedValue is string str && regex.IsMatch(str),
            messageFormatter ?? (value => $"Value \"{value}\" does not match the expected regular expression pattern.")));
    }

    /// <summary>
    /// Adds a rule that passes validation if the string version of the value contains the specified substring.
    /// </summary>
    /// <param name="builder">Builder</param>
    /// <param name="subString">The substring to match.</param>
    /// <param name="comparison">The string comparer to use.</param>
    /// <param name="messageFormatter">A function that formats the message to display when validation fails.</param>
    /// <returns>A reference to this instance.</returns>
    public static ValidatorBuilder<T> Contains<T>(
        this ValidatorBuilder<T> builder,
        string subString,
        StringComparison? comparison = null,
        Func<T, string>? messageFormatter = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(subString);

        return builder.MustSatisfy(new ValidationRule<T>(context =>
                context.AttemptedValue is string str &&
                str.Contains(subString, comparison ?? StringComparison.Ordinal),
            messageFormatter ?? (value => $"Value \"{value}\" does not contain expected substring \"{subString}\".")));
    }

    /// <summary>
    /// Adds a rule that passes validation if the string version of the value begins with the specified substring.
    /// </summary>
    /// <param name="builder">Builder</param>
    /// <param name="subString">The substring to match at the beginning of the value.</param>
    /// <param name="comparison">The string comparison to use.</param>
    /// <param name="messageFormatter">A function that formats the message to display when validation fails.</param>
    /// <returns>A reference to this instance.</returns>
    public static ValidatorBuilder<T> StartsWith<T>(
        this ValidatorBuilder<T> builder,
        string subString,
        StringComparison comparison = StringComparison.Ordinal,
        Func<T, string>? messageFormatter = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(subString);

        return builder.MustSatisfy(new ValidationRule<T>(context =>
                context.AttemptedValue is string str && str.StartsWith(subString, comparison),
            messageFormatter ?? (value => $"Value \"{value}\" does not start with expected substring \"{subString}\".")));
    }

    /// <summary>
    /// Adds a rule that passes validation if the string version of the value ends with the specified substring.
    /// </summary>
    /// <param name="builder">Builder</param>
    /// <param name="subString">The substring to match at the end of the value.</param>
    /// <param name="comparison">The string comparison to use.</param>
    /// <param name="messageFormatter">A function that formats the message to display when validation fails.</param>
    /// <returns>A reference to this instance.</returns>
    public static ValidatorBuilder<T> EndsWith<T>(
        this ValidatorBuilder<T> builder,
        string subString,
        StringComparison comparison = StringComparison.Ordinal,
        Func<T, string>? messageFormatter = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(subString);

        return builder.MustSatisfy(new ValidationRule<T>(context =>
                context.AttemptedValue is string str && str.EndsWith(subString, comparison),
            messageFormatter ?? (value => $"Value \"{value}\" does not end with expected substring \"{subString}\".")));
    }
}