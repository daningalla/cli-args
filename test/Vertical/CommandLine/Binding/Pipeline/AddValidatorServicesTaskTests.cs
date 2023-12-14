using NSubstitute;
using Vertical.CommandLine.Invocation;
using Vertical.CommandLine.Validation;

namespace Vertical.CommandLine.Binding.Pipeline;

public class AddValidatorServicesTaskTests
{
    [Fact]
    public void Invoke_Uses_Symbol_Validator()
    {
        // arrange
        var symbolValidator = CreateValidator();
        var commandValidator = CreateValidator();
        var rootCommand = new RootCommand
        {
            Bindings = { new Switch("--switch", validator: symbolValidator) },
            Validators = { commandValidator },
            Handler = () => { }
        };
        var args = new[] { "--switch=true" };

        // act
        _ = InvocationContextBuilder
            .Create(rootCommand, args)
            .ArgumentProvider
            .GetValue<bool>("--switch");
        
        // assert
        symbolValidator.Received().Validate(Arg.Any<ValidationContext<bool>>());
        commandValidator.DidNotReceive().Validate(Arg.Any<ValidationContext<bool>>());
    }

    [Fact]
    public void Invoke_Uses_Command_Validator()
    {
        // arrange
        var commandValidator = CreateValidator();
        var rootCommand = new RootCommand
        {
            Bindings = { new Switch("--switch") },
            Validators = { commandValidator },
            Handler = () => { }
        };
        var args = new[] { "--switch=true" };

        // act
        _ = InvocationContextBuilder
            .Create(rootCommand, args)
            .ArgumentProvider
            .GetValue<bool>("--switch");
        
        // assert
        commandValidator.Received().Validate(Arg.Any<ValidationContext<bool>>());
    }
    
    [Fact]
    public void Invoke_Uses_Parent_Command_Validator()
    {
        // arrange
        var commandValidator = CreateValidator();
        var rootCommand = new RootCommand
        {
            Commands =
            {
                new Command("command")
                {
                    Handler = () => { },
                    Bindings = { new Switch("--switch") }
                }
            },
            Validators = { commandValidator }
        };
        var args = new[] { "command", "--switch=true" };

        // act
        _ = InvocationContextBuilder
            .Create(rootCommand, args)
            .ArgumentProvider
            .GetValue<bool>("--switch");
        
        // assert
        commandValidator.Received().Validate(Arg.Any<ValidationContext<bool>>());
    }

    private static IValidator<bool> CreateValidator()
    {
        var validator = Substitute.For<IValidator<bool>>();
        validator.ServiceType.Returns(typeof(IValidator<bool>));
        return validator;
    }
}