using System.Text;

namespace Vertical.CommandLine;

internal readonly struct CSharpFormatter
{
    private sealed class StringBuilderWriter
    {
        internal StringBuilderWriter(StringBuilder builder)
        {
            Builder = builder;
        }

        public StringBuilder Builder { get; }
        
        public int Position { get; private set; }

        public bool AtLineOrigin => Position == 0;

        public void Append(char c)
        {
            Builder.Append(c);
            Position++;
        }

        public void Append(string content)
        {
            Builder.Append(content);
            Position += content.Length;
        }

        public void AppendLine()
        {
            Builder.AppendLine();
            Position = 0;
        }
    }
    
    internal struct Separator
    {
        private readonly string _value;
        private int _count;
        
        internal Separator(string value) => _value = value;

        internal string Next() => _count++ > 0 ? _value : string.Empty;
    }
    
    private readonly int _indent;
    private readonly StringBuilderWriter _sb;

    public CSharpFormatter(StringBuilder stringBuilder) : this(new StringBuilderWriter(stringBuilder), 0)
    {
    }

    private CSharpFormatter(
        StringBuilderWriter stringBuilder,
        int indent)
    {
        _sb = stringBuilder;
        _indent = indent;
    }

    private CSharpFormatter GetIndentedFormatter() => new(_sb, _indent + 1);

    public void Append(char c)
    {
        CheckIndent();
        _sb.Append(c);
    }

    public void Append(string content)
    {
        CheckIndent();
        _sb.Append(content);
    }

    public void AppendLine() => _sb.AppendLine();

    public void AppendLine(string content)
    {
        Append(content);
        AppendLine();
    }

    public void AppendBlock(Action<CSharpFormatter> blockFormatter)
    {
        AppendBlock("{", "}", blockFormatter);
    }

    public void AppendBlock(string openToken, string closeToken, Action<CSharpFormatter> blockFormatter)
    {
        NextLine();
        AppendLine(openToken);
        blockFormatter(GetIndentedFormatter());
        NextLine();
        AppendLine(closeToken);
    }

    public void AppendIndented(Action<CSharpFormatter> innerFormatter)
    {
        innerFormatter(GetIndentedFormatter());
    }

    private void NextLine()
    {
        if (_sb.AtLineOrigin)
            return;
        
        AppendLine();
    }

    private void CheckIndent()
    {
        if (!_sb.AtLineOrigin || _indent == 0)
            return;

        for (var c = 0; c < _indent; c++)
            _sb.Append('\t');
    }
}