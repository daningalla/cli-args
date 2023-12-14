using FluentAssertions;

namespace Vertical.CommandLine.Binding.Pipeline;

public class AddOptionValueBindingsTaskTests : BindingContextTest
{
    /// <inheritdoc />
    public AddOptionValueBindingsTaskTests() : base(Setup())
    {
    }

    [Fact]
    public void Invoke_Adds_Simple_Switch_Binding()
    {
        // assert
        AssertBooleanBinding("-a", true);
    }

    [Fact]
    public void Invoke_Adds_Switches_With_Arguments()
    {
        // assert
        AssertBooleanBinding("--arg-true", true);
        AssertBooleanBinding("--arg-false", false);
    }

    [Fact]
    public void Invoke_Adds_Switches_With_Operands()
    {
        // assert
        AssertBooleanBinding("--operand-true", true);
        AssertBooleanBinding("--operand-false", false);

    }

    [Fact]
    public void Invoke_Removes_All_Semantic_Arguments()
    {
        // assert
        Context.GetSemanticArguments().Should().BeEmpty();
    }

    [Fact]
    public void Invoke_Adds_Option_Binding_With_Null()
    {
        // assert
        AssertStringBinding("--default-option", null);
    }

    [Fact]
    public void Invoke_Adds_Option_Binding_With_Argument()
    {
        // assert
        AssertStringBinding("--arg-option", "arg-option-value");
    }

    [Fact]
    public void Invoke_Adds_Option_Binding_With_Operand()
    {
        // assert
        AssertStringBinding("--operand-option", "operand-option-value");
    }

    [Fact]
    public void Invoke_Adds_Option_Binding_With_Empty_MultiValue()
    {
        // assert
        AssertStringMultiBinding("--zero-or-many-none", Enumerable.Empty<string>());
    }

    [Fact]
    public void Invoke_Adds_Option_Binding_With_One_MultiValue()
    {
        // assert
        AssertStringMultiBinding("--zero-or-many-one", new[]{"red"});
    }

    [Fact]
    public void Invoke_Adds_Option_Binding_With_MultiValue()
    {
        // assert
        AssertStringMultiBinding("--zero-or-many-three", new[]{"red", "green", "blue"});
    }

    [Fact]
    public void Invoke_Removes_All_Option_Symbols()
    {
        // assert
        Context.GetBindingSymbols().Should().BeEmpty();
    }

    private static IBindingContext Setup()
    {
        var command = new RootCommand
        {
            Bindings =
            {
                new Switch("-a"),
                new Switch("--arg-true"),
                new Switch("--arg-false"),
                new Switch("--operand-true"),
                new Switch("--operand-false"),
                new Switch("--default"),
                new Option("--default-option"),
                new Option("--arg-option"),
                new Option("--operand-option"),
                new Option("--zero-or-many-none", arity: Arity.ZeroOrMany),
                new Option("--zero-or-many-one", arity: Arity.ZeroOrMany),
                new Option("--zero-or-many-three", arity: Arity.ZeroOrMany),
            },
            Handler = () => { }
        };

        var context = BindingPipeline.CreateContext(command,
            new[]
            {
                "-a",
                "--arg-true", "true",
                "--arg-false", "false",
                "--operand-true=true",
                "--operand-false=false",
                "--arg-option", "arg-option-value",
                "--operand-option=operand-option-value",
                "--zero-or-many-one=red",
                "--zero-or-many-three=red",
                "--zero-or-many-three=green",
                "--zero-or-many-three=blue",
            },
            CancellationToken.None);

        return context;
    }
}