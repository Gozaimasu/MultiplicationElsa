namespace Elsa;

public class ExitException : Exception
{
    public static ExitException Instance = new();
    
    private ExitException() { }
}