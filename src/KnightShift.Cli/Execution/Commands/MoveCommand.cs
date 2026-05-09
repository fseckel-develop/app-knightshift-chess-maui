using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.UseCases;
using KnightShift.Application.UseCases.PlayMove;

namespace KnightShift.Cli.Execution.Commands;

public class MoveCommand : ICommand
{
    private readonly ICommandHandler<PlayMoveCommand> _handler;
    private readonly IMoveSerializer _serializer;

    public CommandInfo Info => new(
        Name: "move",
        Aliases: ["m", "play"],
        Parameter: "{uci}",
        Description: "Play move (e.g. move e2e4)",
        Category: "Game",
        Order: 0
    );

    public MoveCommand(ICommandHandler<PlayMoveCommand> handler, IMoveSerializer serializer)
    {
        _handler = handler;
        _serializer = serializer;
    }

    public bool CanHandle(string input)
    {
        var move = ExtractMove(input);
        return _serializer.TryDeserialize(move, out _);
    }

    public Task<CommandResult> ExecuteAsync(string input)
    {
        try
        {
            var move = ExtractMove(input);
            
            _handler.Handle(new PlayMoveCommand(move));
            
            return Task.FromResult(new CommandResult
            {
                Message = $"Move {move} was played.",
                RefreshGameState = true
            });
        }
        catch (Exception ex)
        {
            return Task.FromResult(new CommandResult
            {
                Message = ex.Message
            });
        }
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
