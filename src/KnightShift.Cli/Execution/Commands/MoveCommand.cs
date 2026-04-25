using KnightShift.Application.Contracts.Interfaces;

namespace KnightShift.Cli.Execution.Commands;

public class MoveCommand : ICommand
{
    private readonly IGameService _game;
    private readonly IMoveSerializer _serializer;

    public CommandInfo Info => new(
        Name: "move",
        Aliases: ["m", "play"],
        Parameter: "{uci}",
        Description: "Play move (e.g. move e2e4)",
        Category: "Game",
        Order: 0
    );

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
        var commandParts = input.Trim()
            .Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

        if (commandParts.Length == 0)
            throw new InvalidOperationException("No input provided.");

        var command = commandParts[0];

        if (IsMoveCommand(command))
        {
            if (commandParts.Length < 2)
                throw new InvalidOperationException("No move provided.");

            return commandParts[1].Trim();
        }

        return command; // assume raw move (default command)
    }

    private bool IsMoveCommand(string command)
    {
        return string.Equals(command, Info.Name, StringComparison.OrdinalIgnoreCase)
            || Info.Aliases.Any(alias => string.Equals(alias, command, StringComparison.OrdinalIgnoreCase));
    }
}
