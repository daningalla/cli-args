using FluentAssertions;

namespace Vertical.CommandLine.Binding;

public class ArgumentIdComparerTests
{
    private readonly IEqualityComparer<string> _instance = BindingNameComparer.Instance;

    [Theory]
    // Positives
    [InlineData("arg", "arg", true)]
    [InlineData("-a", "a", true)]
    [InlineData("--arg", "arg", true)]
    [InlineData("--long-arg", "longarg", true)]
    [InlineData("--long-arg", "longArg", true)]
    [InlineData("--prop", "Prop", true)]
    [InlineData("--prop-arg", "PropArg", true)]
    // Negatives
    public void Compare_Returns_Expected(string x, string y, bool expected)
    {
        _instance.Equals(x, y).Should().Be(expected);
    }
}