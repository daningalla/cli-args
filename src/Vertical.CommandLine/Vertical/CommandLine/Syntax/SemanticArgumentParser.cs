using CommunityToolkit.Diagnostics;
using Vertical.CommandLine.Utilities;

namespace Vertical.CommandLine.Syntax;

/// <summary>
/// Parses input sequences into a <see cref="SemanticArgument"/> collection.
/// </summary>
public sealed class SemanticArgumentParser
{
    private enum QueuePosition { First, Last, Single, Middle }

    private sealed class QueueValue
    {
        internal QueueValue(
            TokenizedInputSequence value,
            QueuePosition position,
            SemanticAnatomy anatomy)
        {
            Value = value;
            Position = position;
            Anatomy = anatomy;
        }

        public TokenizedInputSequence Value { get; }

        public QueuePosition Position { get; }

        public SemanticAnatomy Anatomy { get; }
    }
    
    private readonly List<SemanticArgument> _list = new(32);
    
    /// <summary>
    /// Parses the given input sequences into semantic arguments.
    /// </summary>
    /// <param name="arguments">Arguments.</param>
    /// <returns><see cref="SemanticArgument"/> array.</returns>
    public static SemanticArgument[] Parse(TokenizedInputSequence[] arguments)
    {
        Guard.IsNotNull(arguments);
        
        var instance = new SemanticArgumentParser();

        return instance.ParseInternal(arguments);
    }

    private SemanticArgument[] ParseInternal(TokenizedInputSequence[] arguments)
    {
        var queue = new Queue<TokenizedInputSequence>(arguments);
        var iteration = 0;

        while (queue.TryDequeue(out var sequence))
        {
            var position = (args: arguments.Length, iteration, remaining: queue.Count) switch
            {
                { args: 1 } => QueuePosition.Single,
                { iteration: 0 } => QueuePosition.First,
                { remaining: 0 } => QueuePosition.Last,
                _ => QueuePosition.Middle
            };

            iteration++;
                
            var value = new QueueValue(
                sequence, 
                position,
                SemanticAnatomy.Create(sequence));
            
            if (TryParseTerminatingSequence(sequence))
                break;

            if (TryParsePrefixedSequence(value))
                continue;

            ParseArgumentSequence(sequence);
        }
        
        foreach (var sequence in queue)
        {
            ParseTerminatedArgument(sequence);
        }

        return PostProcessHints(_list);
    }

    private SemanticArgument[] PostProcessHints(IList<SemanticArgument> list)
    {
        SemanticArgument? next = null;

        foreach (var argument in list.Reverse())
        {
            switch (argument)
            {
                case { IsOption: true, HasOperand: false } when next is null:
                case { IsOption: true, HasOperand: false } when next.IsOption:
                    argument.ChangeSemanticHint(SemanticHint.KnownSwitch);
                    break;
            }

            next = argument;
        }
        
        return list.ToArray();
    }

    private void ParseArgumentSequence(TokenizedInputSequence sequence)
    {
        var last = _list.LastOrDefault();
        var precedesOption = last?.IsOption == true;
        var precedesArgOrNothing = last?.IsDiscreetArgument == true || last == null;
        var semanticHint = (precedesOption, precedesArgOrNothing) switch
        {
            { precedesOption: true } => SemanticHint.SpeculativeOperand,
            { precedesArgOrNothing: true } => SemanticHint.DiscreetArgument,
            _ => SemanticHint.None
        };

        _list.Add(new SemanticArgument(Ordinal, sequence, semanticHint));
    }

    private bool TryParsePrefixedSequence(QueueValue queueValue)
    {
        var sequence = queueValue.Value;
        
        // Sequences with one character are never options
        if (sequence.Length == 1)
            return false;

        var format = queueValue.Anatomy.PrefixFormat;

        if (format == IdentifierFormat.None)
            return false;

        return format switch
        {
            IdentifierFormat.Posix => ParsePosixPrefixFormatSequence(queueValue),
            IdentifierFormat.Gnu => ParseGnuPrefixFormatSequence(queueValue),
            _ => ParseMicrosoftPrefixFormatSequence(queueValue)
        };
    }

    private bool ParsePosixPrefixFormatSequence(QueueValue queueValue)
    {
        var anatomy = queueValue.Anatomy;
        
        // Validate format
        var valid = !anatomy.IdentifierSpan.Scan(false, (state, token) => 
            state || !char.IsLetterOrDigit(token.Value));

        if (!valid) return false;

        if (anatomy.IdentifierSpan.Length == 1)
        {
            var hint = (queueValue, Last) switch
            {
                { queueValue.Anatomy.OperandExpression.Length: > 0 } => SemanticHint.None, 
                { queueValue.Position: QueuePosition.Single or QueuePosition.Last } => SemanticHint.KnownSwitch,
                { Last.HasOperand: true } => SemanticHint.KnownSwitch,
                _ => default
            };
            
            // Simple single character option
            _list.Add(new SemanticArgument(Ordinal, queueValue.Value, anatomy, hint));
            return true;
        }
        
        // Multi character group, split off switches
        anatomy.IdentifierSpan[..^1].Scan(token =>
        {
            AddPosixArgument($"-{token.Value}", SemanticHint.KnownSwitch);
        });

        var optionArg = $"-{anatomy.IdentifierSpan[^1].Value}{anatomy.OperandExpression}";
        AddPosixArgument(optionArg, SemanticHint.None);
         
        return true;
    }

    private void AddPosixArgument(string arg, SemanticHint semanticHint)
    {
        var tokens = CharacterTokenLexer.GetTokens(arg);
        var sequence = new TokenizedInputSequence(tokens);
        var prefix = SemanticAnatomy.Create(sequence);
        _list.Add(new SemanticArgument(Ordinal, sequence, prefix, semanticHint));
    }

    private bool ParseGnuPrefixFormatSequence(QueueValue queueValue)
    {
        var anatomy = queueValue.Anatomy;
        
        // Validate format
        var valid = !anatomy.IdentifierSpan.Scan(false, (state, token) =>
            state || !(char.IsLetterOrDigit(token.Value) || token.Value == '-'));

        if (!valid) return false;

        _list.Add(new SemanticArgument(Ordinal, queueValue.Value, anatomy));
        return true;
    }

    private bool ParseMicrosoftPrefixFormatSequence(QueueValue queueValue)
    {
        var anatomy = queueValue.Anatomy;
        
        // Validate format
        var valid = !anatomy.IdentifierSpan.Scan(false, (state, token) =>
            state || !char.IsLetterOrDigit(token.Value));

        if (!valid) return false;

        _list.Add(new SemanticArgument(Ordinal, queueValue.Value, anatomy));
        return true;
    }

    private void ParseTerminatedArgument(TokenizedInputSequence input)
    {
        _list.Add(new SemanticArgument(Ordinal, input, SemanticHint.Terminated));
    }

    private int Ordinal => _list.Count;

    private SemanticArgument? Last => _list.Count > 0 ? _list[^1] : null;

    private static bool TryParseTerminatingSequence(TokenizedInputSequence input)
    {
        return input.Span is
        [
            { Type: CharacterType.TerminatingToken },
            { Type: CharacterType.TerminatingToken }
        ];
    }
}