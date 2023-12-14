using FluentAssertions;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Vertical.CommandLine.Invocation;
using Vertical.CommandLine.Validation;

namespace Vertical.CommandLine.Binding.Pipeline;

public class PipelineValidatorUsageTests
{
    private const int ThrowToken = 0;
    
    [Fact]
    public void Pipeline_Uses_SymbolValidator()
    {
        // arrange
        var validator = CreateValidator();
        var command = new RootCommand
        {
            Bindings = { new Option<int>("--count", validator: validator) },
            Validators = { CreateValidator() },
            Handler = () => { }
        };
        
        // act/assert
        _ = InvocationContextBuilder.Create(command, new[] { $"--count={ThrowToken}" });
        
        // assert
        validator.Received(1).Validate(Arg.Any<IValidationContext<int>>());
    }
    
    [Fact]
    public void Pipeline_Uses_Command_Validator()
    {
        // arrange
        var validator = CreateValidator();
        var command = new RootCommand
        {
            Bindings = { new Option<int>("--count") },
            Validators = { validator },
            Handler = () => { }
        };
        
        // act/assert
        _ = InvocationContextBuilder.Create(command, new[] { $"--count={ThrowToken}" });
        
        // assert
        validator.Received(1).Validate(Arg.Any<IValidationContext<int>>());
    }

    [Fact]
    public void Pipeline_Uses_Propagated_Command_Validator()
    {
        // arrange
        var validator = CreateValidator();
        var command = new RootCommand
        {
            Validators = { validator },
            Commands =
            {
                new Command("cmd")
                {
                    Bindings = { new Option<int>("--count") },
                    Handler = () => { }
                }
            }
        };
        
        // act/assert
        _ = InvocationContextBuilder.Create(command, new[] { "cmd", $"--count={ThrowToken}" });
        
        // assert
        validator.Received(1).Validate(Arg.Any<IValidationContext<int>>());

    }

    private static IValidator<int> CreateValidator()
    {
        var validator = Substitute.For<IValidator<int>>();
        validator.ServiceType.Returns(typeof(IValidator<int>));
        return validator;
    }
}