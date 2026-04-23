using KnightShift.Application.Contracts.Interfaces;

namespace KnightShift.Cli.Execution.Commands;

public class MoveCommand : ICommand
{
    private readonly IGameService _game;
    private readonly IMoveSerializer _serializer;

    public string Name => "move";

    public MoveCommand(IGameService game, IMoveSerializer serializer)
    {
        _game = game;
        _serializer = serializer;
    }

    public bool CanHandle(string input)
    {
        var move = ExtractMove(input);
        return _serializer.TryDeserialize(move, out _);
    }

    public Task ExecuteAsync(string input)
    {
        try
        {
            var move = ExtractMove(input);
            _game.ApplyMove(move);
            Console.WriteLine($"Move played: {move}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Invalid move: {ex.Message}");
        }
        return Task.CompletedTask;
    }

    private string ExtractMove(string input)
    {
        var trimmed = input.Trim();
        if (trimmed.StartsWith(Name, StringComparison.OrdinalIgnoreCase))
            return trimmed[5..].Trim();

        return trimmed;
    }
}
