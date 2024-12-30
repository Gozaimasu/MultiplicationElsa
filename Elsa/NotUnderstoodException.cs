namespace Elsa;

public class NotUnderstoodException : Exception
{
    public static NotUnderstoodException Instance = new();
    
    private NotUnderstoodException() { }
}