using System.Text;

namespace Vertical.CommandLine.Utilities;

internal static class ReusableStringBuilder
{
    private static readonly Reusable<StringBuilder> Instance = new(
        () => new StringBuilder(1000),
        sb => sb.Clear());

    internal static string Build(Action<StringBuilder> writer)
    {
        using var leased = GetInstance();
        
        writer(leased.Value);
        var content = leased.Value.ToString();

        return content;
    }

    private static Reusable<StringBuilder>.LeasedValue GetInstance() => Instance.GetInstance();
}