using FluentAssertions;

namespace Vertical.CommandLine.Utilities;

public class PathVisitorTests
{
    [Fact]
    public void Visit_Returns_Expected_Visits()
    {
        var command = new RootCommand
        {
            Commands =
            {
                new Command("red"),
                new Command("green")
                {
                    Commands =
                    {
                        new Command("yellow"){ Commands = { new Command("violet" )}},
                        new Command("pink"),
                        new Command("brown")
                        {
                            Commands =
                            {
                                new Command("cyan"),
                                new Command("magenta")
                            }
                        }
                    }
                },
                new Command("blue")
                {
                    Commands = { new Command("orange") }
                }
            }
        };

        var paths = new List<string>();
        var visitor = new PathVisitor<Command>(cmd => cmd.Commands, path => paths.Add(
            string.Join("->", path.Select(p => p.Id))));
        
        visitor.Visit(command);

        paths.Should().Equal(
            "(root)->red",
            "(root)->green->yellow->violet",
            "(root)->green->pink",
            "(root)->green->brown->cyan",
            "(root)->green->brown->magenta",
            "(root)->blue->orange");
    }
}