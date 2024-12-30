namespace Elsa;

public class WrongAnswerException : Exception
{
    public static WrongAnswerException Instance = new();
    
    private WrongAnswerException() { }
}