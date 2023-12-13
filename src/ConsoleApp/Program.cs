// See https://aka.ms/new-console-template for more information
using Vertical.CommandLine;
using Vertical.CommandLine.Validation;

Console.WriteLine("Hello, World!");

var command = new RootCommand
{
    Commands =
    {
        new Command("build")
        {
            Handler = async (
                FileInfo project, 
                bool noRestore,
                string source,
                DirectoryInfo? outputPath,
                CancellationToken cancellationToken) =>
            {
                await Task.CompletedTask;
                return 0;
            },
            Bindings =
            {
                new Argument<FileInfo>("project", arity: Arity.One),
                new Switch("--no-restore"),
                new Option<string>("--source", validator: Validator.Build<string>(rules => rules.MinimumLength(5))),
                new Option<DirectoryInfo>("--output-path")
            }
        }
    }
};

args = new[]
{
    "build",
    "Vertical.CommandLine.csproj",
    "--no-restore",
    "--source=$PROFILE/.nuget/packages"
};

try
{
    await command.InvokeAsync(args, CancellationToken.None);
}
catch (CommandLineException exception)
{
    Console.WriteLine(exception.Message);
}


