using FluentAssertions;
using Vertical.CommandLine.Test;

namespace Vertical.CommandLine.Binding.Pipeline;

public class BuildCommandPathTaskTests
{
    private readonly IBindingTask _instance = new BuildCommandPathTask();
    
    [Theory, MemberData(nameof(Invoke_Finds_Correct_Invocation_Subject_Theories))]
    public void Invoke_Finds_Correct_Invocation_Subject(
        RootCommand rootCommand,
        string[] args,
        string expectedId)
    {
        // arrange
        var context = new BindingContext(rootCommand, args, CancellationToken.None);
        
        // act
        _instance.Invoke(context, BindingPipeline.TerminalTask);
        
        // assert
        context.InvocationSubject.Id.Should().Be(expectedId);
    }

    public static IEnumerable<object[]> Invoke_Finds_Correct_Invocation_Subject_Theories => TheoryBuilder.Build(t => t
        .AddCase(new RootCommand(), Array.Empty<string>(), RootCommand.RootId)
        .AddCase(new RootCommand { Commands = { new Command("child") } }, Array.Empty<string>(), RootCommand.RootId)
        .AddCase(new RootCommand
            {
                Commands =
                {
                    new Command("child-1"),
                    new Command("child-2")
                }
            },
            new[] { "child-1" }, "child-1")
        .AddCase(new RootCommand
            {
                Commands = { new Command("child-1") { Commands = { new Command("child-2") } } }
            },
            new[] { "child-1", "child-2" },
            "child-2")
    );

    [Theory, MemberData(nameof(Invoke_Sets_Correct_Binding_Values_Theories))]
    public void Invoke_Sets_Correct_Binding_Values(
        RootCommand rootCommand,
        string[] args,
        string[] expectedArgs)
    {
        // arrange
        var context = new BindingContext(rootCommand, args, CancellationToken.None);
        
        // act
        _instance.Invoke(context, BindingPipeline.TerminalTask);
        
        // assert
        context.InvocationArgumentValues.Should().Equal(expectedArgs);
    }

    public static IEnumerable<object[]> Invoke_Sets_Correct_Binding_Values_Theories => TheoryBuilder.Build(t => t
        .AddCase(new RootCommand(), new[] { "1", "2", "3" }, new[] { "1", "2", "3" })
        .AddCase(new RootCommand { Commands = { new Command("child") } }, new[] { "1", "2", "3" },
            new[] { "1", "2", "3" })
        .AddCase(new RootCommand { Commands = { new Command("child") } }, new[] { "child", "1", "2" },
            new[] { "1", "2" })
        .AddCase(new RootCommand { Commands = { new Command("child") } }, new[] { "--", "child", "1", "2" }, 
            new[] { "--", "child", "1", "2" })
    );
}