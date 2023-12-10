using FluentAssertions;

namespace Vertical.CommandLine.Utilities;

public class NamingUtilitiesTests
{
    [Theory]
    [InlineData("n", "n")]
    [InlineData("red", "red")]
    [InlineData("-r", "r")]
    [InlineData("--red", "red")]
    [InlineData("--red-color", "redColor")]
    [InlineData("--red-color-value", "redColorValue")]
    public void GetInferredBindingName_Returns_Expected(string id, string expected)
    {
        NamingUtilities.GetInferredBindingName(id).Should().Be(expected);
    }
}