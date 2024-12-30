namespace Elsa;

public class WrongAnswerException : Exception
{
    public int RightAnswer { get; }

    public WrongAnswerException(int rightAnswer)
    {
        RightAnswer = rightAnswer;
    }
}