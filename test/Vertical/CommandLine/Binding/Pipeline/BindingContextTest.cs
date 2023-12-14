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
        value.Should().Be(expected);
    }

    protected void AssertStringBinding(string symbol, string? expected)
    {
        // arrange
        var bindings = Context.GetValueBindings().ToDictionary(binding => binding.BindingId);
        
        // act
        

        // assert
        
    }

    protected void AssertStringMultiBinding(string symbol, IEnumerable<string> expected)
    {
        var bindings = Context.GetValueBindings().ToDictionary(binding => binding.BindingId);
        var binding = (ArgumentValueBinding<string>)bindings[symbol];

        binding.ArgumentValues.Should().Equal(expected);
    }
}