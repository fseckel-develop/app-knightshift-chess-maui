using KnightShift.Application.Contracts.DTOs;
using KnightShift.Application.UseCases;
using KnightShift.Application.UseCases.RedoMove;

namespace KnightShift.Cli.Execution.Commands;

public class RedoCommand : ICommand
{
    private readonly IQueryHandler<RedoMoveCommand, MoveDto?> _handler;

    public CommandInfo Info => new(
        Name: "redo",
        Aliases: ["r"],
        Parameter: null,
        Description: "Redo last move",
        Category: "Game",
        Order: 2
    );

    public RedoCommand(IQueryHandler<RedoMoveCommand, MoveDto?> handler)
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
            var redoneMove = _handler.Handle(new RedoMoveCommand());

            return Task.FromResult(new CommandResult
            {
                Message = $"Move {redoneMove!.Origin}{redoneMove!.Target} redone.",
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
