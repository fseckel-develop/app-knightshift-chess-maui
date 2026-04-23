using KnightShift.Application.Contracts.Interfaces;

namespace KnightShift.Cli.Execution.Commands;

public class ListMovesCommand : ICommand
{
    private readonly IGameService _game;

    public string Name => "list";

    public ListMovesCommand(IGameService game)
    {
        _game = game;
    }

    public bool CanHandle(string input)
        => input.StartsWith(Name, StringComparison.OrdinalIgnoreCase);

    public Task ExecuteAsync(string input)
    {
        try
        {
            var listAndSquare = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var moves = (listAndSquare.Length == 2)
                ? _game.GetLegalMoves(listAndSquare[1])
                : _game.GetLegalMoves();

            if (!moves.Any())
            {
                Console.WriteLine("No legal moves.");
                return Task.CompletedTask;
            }

            foreach (var move in moves)
            {
                Console.Write($"{move.Origin}{move.Target} ");
            }
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return Task.CompletedTask;
    }
}
