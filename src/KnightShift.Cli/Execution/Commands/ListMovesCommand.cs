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
        => input.Equals(Name, StringComparison.OrdinalIgnoreCase);

    public Task ExecuteAsync(string input)
    {
        var moves = _game.GetLegalMoves();

        if (!moves.Any())
        {
            Console.WriteLine("No legal moves.");
            return Task.CompletedTask;
        }

        Console.WriteLine("Legal moves:");

        foreach (var move in moves)
        {
            Console.Write($"{move.Origin}{move.Target} ");
        }

        Console.WriteLine();
        return Task.CompletedTask;
    }
}
