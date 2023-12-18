using FluentAssertions;

namespace Vertical.CommandLine.Binding.Pipeline;

public abstract class BindingContextTest
{
    protected readonly IBindingContext Context;

    protected BindingContextTest(IBindingContext context)
    {
        Context = context;
    }
    
    protected void AssertBooleanBinding(string symbol, bool expected)
    {
        // arrange
        var bindings = Context.GetValueBindings().ToDictionary(binding => binding.BindingId);
        var binding = (ArgumentValueBinding<bool>)bindings[symbol];
        
        // act
        var value = binding.ArgumentValues.First();
        
        // assert
        value.Should().Be(expected);
    }

    protected void AssertStringBinding(string symbol, string? expected)
    {
        // arrange
        var bindings = Context.GetValueBindings().ToDictionary(binding => binding.BindingId);
        var binding = (ArgumentValueBinding<string>)bindings[symbol];

        // act
        var value = binding.ArgumentValues.First();

        // assert
        value.Should().Be(expected);
    }

    protected void AssertStringMultiBinding(string symbol, IEnumerable<string> expected)
    {
        // arrange/act
        var bindings = Context.GetValueBindings().ToDictionary(binding => binding.BindingId);
        var binding = (ArgumentValueBinding<string>)bindings[symbol];

        // assert
        binding.ArgumentValues.Should().Equal(expected);
    }
}