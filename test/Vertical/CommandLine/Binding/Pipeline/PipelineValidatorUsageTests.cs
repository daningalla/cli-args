using FluentAssertions;
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
        var command = new RootCommand
        {
            Bindings = { new Option<int>("--count", configureValidator: ConfigureValidator(i => i != ThrowToken)) },
            Validators = { CreateValidator(i => i == ThrowToken) },
            Handler = () => { }
        };
        
        // act/assert
        var context = InvocationContextBuilder.Create(command, new[] { $"--count={ThrowToken}" });
        var argProvider = context.ArgumentProvider;
        
        // assert
        argProvider
            .Invoking(x => x.GetValue<int>("--count"))
            .Should()
            .Throw<CommandLineException>();
    }
    
    [Fact]
    public void Pipeline_Uses_Command_Validator()
    {
        // arrange
        var command = new RootCommand
        {
            Bindings = { new Option<int>("--count") },
            Validators = { CreateValidator(i => i != ThrowToken) },
            Handler = () => { }
        };
        
        // act/assert
        var context = InvocationContextBuilder.Create(command, new[] { $"--count={ThrowToken}" });
        var argProvider = context.ArgumentProvider;
        
        // assert
        argProvider
            .Invoking(x => x.GetValue<int>("--count"))
            .Should()
            .Throw<CommandLineException>();
    }

    [Fact]
    public void Pipeline_Uses_Propagated_Command_Validator()
    {
        // arrange
        var command = new RootCommand
        {
            Validators = { CreateValidator(i => i != ThrowToken) },
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
        var context = InvocationContextBuilder.Create(command, new[] { "cmd", $"--count={ThrowToken}" });
        var argProvider = context.ArgumentProvider;
        
        // assert
        argProvider
            .Invoking(x => x.GetValue<int>("--count"))
            .Should()
            .Throw<CommandLineException>();
    }

    private static IValidator<int> CreateValidator(Func<int, bool> predicate)
    {
        return new Validator<int>(new[] { new ValueConstraint<int>(predicate, null) });
    }

    private static Action<IValidationBuilder<int>> ConfigureValidator(Func<int, bool> predicate)
    {
        return builder => builder.Must(predicate);
    } 
}