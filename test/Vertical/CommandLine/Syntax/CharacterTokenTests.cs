using Vertical.CommandLine.Test;

namespace Vertical.CommandLine.Syntax;

public class CharacterTokenTests
{
    [Fact]
    public void Equality_Returns_True()
    {
        var (x, y) = (
            new CharacterToken("text", 0, CharacterType.IdentifierToken),
            new CharacterToken("text", 0, CharacterType.IdentifierToken));

        EqualityTest.AssertEqualityOperations(x, y, x == y, x != y);
    }
}