// See https://aka.ms/new-console-template for more information

using ConsoleApp;
using Vertical.CommandLine;

var rootCommand = Setup.BuildRootCommand();

try
{
    args = new[]
    {
        "nuget",
        "push",
        "/usr/src/vertical.commandline.csproj",
        "--api-key=annsiju9889792hkjhdb82730i2hj9j292oth939",
        "--skip-duplicates",
        "--timeout=00:00:30"
    };
    await rootCommand.InvokeAsync(args, CancellationToken.None);
}
catch (CommandLineException exception)
{
    Console.WriteLine(exception.Message);
}


