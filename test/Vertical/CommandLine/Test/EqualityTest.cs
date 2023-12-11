using FluentAssertions;

namespace Vertical.CommandLine.Test;

public static class EqualityTest
{
    public static void AssertEqualityOperations<T>(T x, T y, bool opEquals, bool opNotEquals) where T : notnull
    {
        x.Equals(y).Should().BeTrue();
        x.GetHashCode().Should().Be(y.GetHashCode());
        opEquals.Should().BeTrue();
        opNotEquals.Should().BeFalse();
    }
}