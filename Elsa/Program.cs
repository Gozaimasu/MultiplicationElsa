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

List<int> operands = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
var errors = 0;
do
{
    var rand = Random.Shared.Next(operands.Count);

    do
    {
        var mul2 = operands[rand];
        operands.RemoveAt(rand);

        Console.Clear();

        (string Message, bool Again) result = AskQuestion(mul1, mul2)
            .Match(
                _ => ("Bravo Elsa", false),
                error =>
                {
                    if (error is not WrongAnswerException wae)
                        return ("Je n'ai pas compris.", true);

                    errors++;
                    operands.Add(mul2);
                    return ($"Non, la réponse était {wae.RightAnswer}.", false);
                });

        // Affichage du message
        Console.WriteLine(result.Message);

        // En cas d'incompréhension, on repose la même question
        if (!result.Again)
        {
            rand = Random.Shared.Next(operands.Count);
        }
        else
        {
            operands.Insert(rand, mul2);
        }

        Console.ReadLine();
    } while (operands.Count > 0);

    Console.Clear();
    Console.WriteLine(errors == 0 ? $"Tu n'as pas d'erreurs." : $"Tu as fait {errors} erreurs.");

    if (errors <= 5)
    {
        Console.Write("*");
        Thread.Sleep(TimeSpan.FromSeconds(1));
    }

    if (errors <= 2)
    {
        Console.Write("*");
        Thread.Sleep(TimeSpan.FromSeconds(1));
    }

    if (errors == 0)
    {
        Console.WriteLine("*");
        Console.WriteLine("Bravo Elsa, tu as fait tout juste!!!");
    }
    else
    {
        Console.WriteLine();
        Console.WriteLine("Essaye de faire mieux la prochaine fois");
    }

    if (!AskContinue())
    {
        Console.WriteLine("Au revoir Elsa");
        return;
    }
    
    operands = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
    errors = 0;
} while (true);

Result<Unit> AskQuestion(int op1, int op2)
{
    Console.WriteLine($"Combien font {op1} x {op2}?");
    var response = Console.ReadLine();

    if (!int.TryParse(response, out var result))
        return new Result<Unit>(NotUnderstoodException.Instance);

    if (result == op1 * op2) return Prelude.unit;

    var e = new WrongAnswerException(op1 * op2);
    return new Result<Unit>(e);
}

bool AskContinue()
{
    Console.WriteLine("On continue? ([O]ui/[N]on)");
    var quitter = Console.ReadKey();
    Console.WriteLine();

    return quitter.Key == ConsoleKey.O;
}