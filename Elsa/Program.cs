using Elsa;
using LanguageExt;
using LanguageExt.Common;

int mul1;
do
{
    Console.Clear();

    Console.WriteLine("Quelle table veux-tu travailler Elsa?");

    var key = Console.ReadKey();
    Console.WriteLine();
    if (key.Key is ConsoleKey.D2 or ConsoleKey.D3 or ConsoleKey.D4 or ConsoleKey.D5)
    {
        mul1 = key.Key - ConsoleKey.D0;
        break;
    }

    Console.WriteLine("Je n'ai pas compris.");
    Thread.Sleep(TimeSpan.FromSeconds(5));
} while (true);

do
{
    var mul2 = Random.Shared.Next(10);

    do
    {
        Console.Clear();

        var result = AskQuestion(mul1, mul2)
            .Match(
                _ => ("Bravo Elsa", false, false),
                error => error switch
                {
                    ExitException => ("Au revoir Elsa.", true, false),
                    NotUnderstoodException => ("Je n'ai pas compris.", false, true),
                    WrongAnswerException => ("Non ce n'est pas ça.", false, true),
                    _ => ("Il y a eu un problème", true, false)
                });

        Console.WriteLine(result.Item1);
        if (result.Item2) return;
        if (!result.Item3) break;
        Thread.Sleep(TimeSpan.FromSeconds(5));
    } while (true);

    if (!AskContinue())
    {
        Console.WriteLine("Au revoir Elsa");
        return;
    }
} while (true);

Result<Unit> AskQuestion(int op1, int op2)
{
    Console.WriteLine($"Combien font {op1} x {op2}?");
    Console.WriteLine("Donne ta réponse et appuie sur Entrée (q pour quitter)");
    var response = Console.ReadLine();

    if (response == "q")
        return new Result<Unit>(ExitException.Instance);

    if (!int.TryParse(response, out var result))
        return new Result<Unit>(NotUnderstoodException.Instance);

    return result == op1 * op2
        ? Prelude.unit
        : new Result<Unit>(WrongAnswerException.Instance);
}

bool AskContinue()
{
    Console.WriteLine("On continue? ([O]ui/[N]on)");
    var quitter = Console.ReadKey();
    Console.WriteLine();

    return quitter.Key == ConsoleKey.O;
}