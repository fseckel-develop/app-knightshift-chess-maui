using KnightShift.Application.Contracts.DTOs;
using KnightShift.Application.UseCases;
using KnightShift.Application.UseCases.GetMoves;
using KnightShift.Cli.Rendering.State;

namespace KnightShift.Cli.Execution.Commands;

public class ListCommand : ICommand
{
    private readonly IQueryHandler<GetMovesQuery, IEnumerable<MoveDto>> _handler;

    public CommandInfo Info => new(
        Name: "list",
        Aliases: ["moves"],
        Parameter: "[square]",
        Description: "List legal moves",
        Category: "View",
        Order: 0
    );

    public ListCommand(IQueryHandler<GetMovesQuery, IEnumerable<MoveDto>> handler)
    {
        _handler = handler;
    }

    public bool CanHandle(string input)
    {
        return input.StartsWith(Info.Name, StringComparison.OrdinalIgnoreCase) ||
            Info.Aliases.Any(alias => input.StartsWith(alias, StringComparison.OrdinalIgnoreCase));
    }

    public Task<CommandResult> ExecuteAsync(string input)
    {
        try
        {
            var commandParts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string? origin = (commandParts.Length == 2) ? commandParts[1] : null;

            var moves = _handler.Handle(new GetMovesQuery(origin)).ToList();
            
            var noun = (moves.Count == 1) ? "move" : "moves";
            var suffix = origin is not null ? $" from {origin}" : "";

            return Task.FromResult(new CommandResult
            {
                ContentType = UiContent.Moves,
                ContentState = new MovesContentState { OriginSquare = origin },
                Message = (moves.Count == 0)
                    ? $"Found no legal moves{suffix}."
                    : $"Found {moves.Count} legal {noun}{suffix}."
            });
        }
        catch (Exception ex)
        {
            return Task.FromResult(new CommandResult
            {
                ContentType = UiContent.Moves,
                Message = ex.Message
            });
        }
    }
}
