using FluentAssertions;
using Vertical.CommandLine.Test;

namespace Vertical.CommandLine.Configuration;

public class ArityTests
{
    [Fact]
    public void Test_Equality()
    {
        // arrange
        var (x, y) = (Arity.One, Arity.One);
        
        // assert
        EqualityTest.AssertEqualityOperations(x, y, x == y, x != y);
    }

    [Fact]
    public void Arity_Static_Definition_Returns_Expected_Properties()
    {
        // assert
        (Arity.ZeroOrMany is { MinCount: 0, MaxCount: null }).Should().BeTrue();
        (Arity.ZeroOrOne is { MinCount: 0, MaxCount: 1 }).Should().BeTrue();
        (Arity.One is { MinCount: 1, MaxCount: 1 }).Should().BeTrue();
        (Arity.OneOrMany is { MinCount: 1, MaxCount: null }).Should().BeTrue();
        (Arity.Exactly(2) is { MinCount: 2, MaxCount: 2 }).Should().BeTrue();
    }

    [Fact]
    public void Arity_Ctor_Throws_When_Invalid()
    {
        // assert
        new Action(() => new Arity(-1, 0)).Invoking(a => a()).Should().Throw<ArgumentException>();
        new Action(() => new Arity(0, -1)).Invoking(a => a()).Should().Throw<ArgumentException>();
    }
}