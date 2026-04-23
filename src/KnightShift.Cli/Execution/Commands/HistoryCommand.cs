using KnightShift.Application.Contracts.Interfaces;

namespace KnightShift.Cli.Execution.Commands;

public class HistoryCommand : ICommand
{
    private readonly IGameService _game;

    public string Name => "history";

    public HistoryCommand(IGameService game)
    {
        _game = game;
    }

    public bool CanHandle(string input)
        => input.Equals(Name, StringComparison.OrdinalIgnoreCase);

    public Task ExecuteAsync(string input)
    {
        var moves = _game.GetMoveHistoryFormatted().ToList();

        if (moves.Count == 0)
        {
            Console.WriteLine("No moves played.");
            return Task.CompletedTask;
        }

        for (int i = 0; i < moves.Count; i += 2)
        {
            int moveNumber = i / 2 + 1;

            Console.Write($"{moveNumber}. {moves[i]}");

            if (i + 1 < moves.Count)
                Console.Write($" {moves[i + 1]}");

            Console.WriteLine();
        }

        return Task.CompletedTask;
    }
}
