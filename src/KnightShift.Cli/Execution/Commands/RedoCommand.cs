using KnightShift.Application.Contracts.Interfaces;

namespace KnightShift.Cli.Execution.Commands;

public class RedoCommand : ICommand
{
    private readonly IGameService _game;

    public CommandInfo Info => new(
        Name: "redo",
        Aliases: ["r"],
        Parameter: null,
        Description: "Redo last move",
        Category: "Game",
        Order: 2
    );

    public RedoCommand(IGameService game)
    {
        _game = game;
    }

    public bool CanHandle(string input)
    {
        return input.Equals(Info.Name, StringComparison.OrdinalIgnoreCase) ||
            Info.Aliases.Any(alias => input.Equals(alias, StringComparison.OrdinalIgnoreCase));
    }

    public Task<CommandResult> ExecuteAsync(string input)
    {
        try
        {
            _game.RedoMove();
            var state = _game.GetState();

            var move = state.LastMove!;

            return Task.FromResult(new CommandResult
            {
                Message = $"Move {move.Origin}{move.Target} redone.",
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
}
