using Vertical.CommandLine.Syntax;

namespace Vertical.CommandLine.Binding.Pipeline;

public class PrepareSemanticArgumentsTask : IBindingTask
{
    /// <inheritdoc />
    public void Invoke(IBindingContext context, Action<IBindingContext> next)
    {
        InvokeCore(context);
        next(context);
    }

    private static void InvokeCore(IBindingContext context)
    {
        var inputSequences = context
            .InvocationArgumentValues
            .Select(CharacterTokenLexer.GetTokens)
            .Select(tokens => new TokenizedInputSequence(tokens))
            .ToArray();

        var arguments = SemanticArgumentParser.Parse(inputSequences);
        
        context.StageSemanticArguments(arguments);
    }
}