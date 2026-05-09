using KnightShift.Application.UseCases.UndoMove;

namespace KnightShift.Cli.Execution.Commands;

public class UndoCommand : ICommand
{
    private readonly UndoMoveHandler _handler;

    public CommandInfo Info => new(
        Name: "undo",
        Aliases: ["u"],
        Parameter: null,
        Description: "Undo last move",
        Category: "Game",
        Order: 1
    );

    public UndoCommand(UndoMoveHandler handler)
    {
        _handler = handler;
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
            var undoneMove = _handler.Handle(new UndoMoveCommand());

            return Task.FromResult(new CommandResult
            {
                Message = $"Move {undoneMove!.Origin}{undoneMove!.Target} undone.",
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
