using FluentAssertions;

namespace Vertical.CommandLine.Validation;

public class ValidatorTests
{
    [Fact]
    public void Build_Invokes_Action()
    {
        var validator = Validator.Build<int>(builder => builder.Not(0));
        validator.Should().NotBeNull();
    }
}