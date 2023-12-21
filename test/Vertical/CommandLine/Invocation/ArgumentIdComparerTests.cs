using FluentAssertions;

namespace Vertical.CommandLine.Invocation;

public class ArgumentIdComparerTests
{
    private readonly IEqualityComparer<ArgumentId> _instance = new ArgumentIdComparer();

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
        var idX = new ArgumentId(x);
        var idY = new ArgumentId(y);

        _instance.Equals(idX, idY).Should().Be(expected);
    }
}