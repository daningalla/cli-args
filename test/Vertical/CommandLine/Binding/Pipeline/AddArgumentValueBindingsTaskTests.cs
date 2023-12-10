namespace Vertical.CommandLine.Binding.Pipeline;

public class AddArgumentValueBindingsTaskTests : BindingContextTest
{
    /// <inheritdoc />
    public AddArgumentValueBindingsTaskTests() : base(Setup())
    {
    }

    [Fact]
    public void Invoke_Finds_Shape_Binding()
    {
        // assert
        AssertStringBinding("shape", "square");
    }

    [Fact]
    public void Invoke_Finds_Size_Binding()
    {
        // assert
        AssertStringBinding("size", "xs");
    }

    [Fact]
    public void Invoke_Finds_Colors_Binding()
    {
        // assert
        AssertStringMultiBinding("colors", new[]{"red", "green", "blue"});
    }

    private static IBindingContext Setup()
    {
        var command = new RootCommand
        {
            Bindings =
            {
                new Switch("-d"),
                new Option("--config", arity: Arity.ZeroOrMany),
                
                // True test cases
                new Argument("shape", arity: Arity.One),
                new Argument("size", arity: Arity.One),
                new Argument("colors", arity: Arity.ZeroOrMany)
            },
            Handler = () => { }
        };

        var args = new[]
        {
            "square",
            "xs",
            "red", "green", "blue",
            "-d",
            "--config",
            "release"
        };

        return BindingPipeline.CreateContext(command, args, CancellationToken.None);
    }
}