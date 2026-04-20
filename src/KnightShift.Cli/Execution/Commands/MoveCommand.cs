using KnightShift.Application.Contracts.Interfaces;

namespace KnightShift.Cli.Execution.Commands;

public class MoveCommand : ICommand
{
    private readonly IGameService _game;

    public string Name => "move";

    public MoveCommand(IGameService game)
    {
        _game = game;
    }

    public bool CanHandle(string input)
    {
        return input.StartsWith(Name, StringComparison.OrdinalIgnoreCase);
    }

    public Task ExecuteAsync(string input)
    {
        try
        {
            var move = input[4..].Trim();
            _game.ApplyMove(move);
            Console.WriteLine($"Move played: {move}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Invalid move: {ex.Message}");
        }
        return Task.CompletedTask;
    }
}
