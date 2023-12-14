using FluentAssertions;
using Vertical.CommandLine.Invocation;

namespace Vertical.CommandLine.Binding.Pipeline;

public class AddModelBindingTaskTests
{
    internal enum BuildConfig { Debug, Release }

    internal record BuildParameters(BuildConfig Configuration, FileInfo ProjectPath, bool NoRestore);

    internal class BuildParametersBinder : ModelBinder<BuildParameters>
    {
        /// <inheritdoc />
        protected override IModelValue<BuildParameters> BindInstance(IMappedArgumentProvider argumentProvider)
        {
            var value = new BuildParameters(
                argumentProvider.GetValue<BuildConfig>("--config"),
                argumentProvider.GetValue<FileInfo>("project"),
                argumentProvider.GetValue<bool>("--no-restore"));

            return new ModelValue<BuildParameters>(value);
        }
    }

    [Fact]
    public void Invoke_Invokes_Model_Binder()
    {
        // arrange
        const string path = "/usr/src/vertical.commandline.csproj";
        var rootCommand = new RootCommand
        {
            Bindings =
            {
                new Option<BuildConfig>("--config"),
                new Switch("--no-restore"),
                new Argument<FileInfo>("project")
            },
            ModelBinders = { new BuildParametersBinder() },
            Handler = (BuildParameters parameters) => { }
        };
        
        string[] args = { "/usr/src/vertical.commandline.csproj", "--config", "Release", "--no-restore" };
        
        // act
        var context = InvocationContextBuilder.Create(rootCommand, args);
        var provider = context.ArgumentProvider;
        var model = provider.GetValue<BuildParameters>("parameters");
        
        // assert
        model.Configuration.Should().Be(BuildConfig.Release);
        model.NoRestore.Should().BeTrue();
        model.ProjectPath.FullName.Should().Be(new FileInfo(path).FullName);
    }
}