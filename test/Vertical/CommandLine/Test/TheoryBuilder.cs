namespace Vertical.CommandLine.Test;

internal static class TheoryBuilder
{
    internal sealed class ParamsBuilder
    {
        internal List<object[]> Data { get; } = new();

        internal ParamsBuilder AddCase(params object[] data)
        {
            Data.Add(data);
            return this;
        }
    }
    
    internal static IEnumerable<object[]> Build(Action<ParamsBuilder> builder)
    {
        var paramsBuilder = new ParamsBuilder();
        builder(paramsBuilder);
        return paramsBuilder.Data;
    }
}