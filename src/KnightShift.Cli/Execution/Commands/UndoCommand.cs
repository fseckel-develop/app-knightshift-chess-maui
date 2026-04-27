using KnightShift.Application.Contracts.Interfaces;

namespace KnightShift.Cli.Execution.Commands;

public class UndoCommand : ICommand
{
    private readonly IGameService _game;

    public CommandInfo Info => new(
        Name: "undo",
        Aliases: ["u"],
        Parameter: null,
        Description: "Undo last move",
        Category: "Game",
        Order: 1
    );

    public UndoCommand(IGameService game)
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
            var state = _game.GetState();
            _game.UndoMove();

            return Task.FromResult(new CommandResult
            {
                Message = $"Move {state.LastMove!.Origin}{state.LastMove!.Target} undone.",
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
