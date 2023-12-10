// See https://aka.ms/new-console-template for more information
using Vertical.CommandLine;

Console.WriteLine("Hello, World!");

var match = new[] { 1, 2, 3 } is [1,2];

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
                new Option("--source"),
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


