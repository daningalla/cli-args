namespace Vertical.CommandLine;

internal sealed class NamingManager
{
    private readonly Dictionary<Guid, string> _registrations = new();
    private readonly HashSet<string> _names = new();
    
    public string GetHandlerClrCompliantName(HandlerMetadata metadata)
    {
        if (_registrations.TryGetValue(metadata.Id, out var clrName))
            return clrName;

        var clrIdentifier = GetCandidateIdentifier(metadata.CommandId);
        var candidateName = clrIdentifier;
        var iteration = 2;

        while (_names.Contains(candidateName))
        {
            candidateName = $"{clrIdentifier}{iteration++}";
        }

        _names.Add(candidateName);
        _registrations.Add(metadata.Id, candidateName);

        return candidateName;
    }
    
    private static string GetCandidateIdentifier(string commandId)
    {
        if (commandId == Constants.RootCommandId)
            return "Root";
        
        return string.Join(
            string.Empty,
            commandId.Replace("--", string.Empty).Split('-').Select(Utilities.Capitalize));
    }
}