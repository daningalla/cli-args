namespace Vertical.CommandLine.Validation;

public class ValidatorBuilder<T>
{
    private readonly List<(Func<T, bool> validator, Func<T, string> messageProvider)> _rules = new(4);
    
     
}