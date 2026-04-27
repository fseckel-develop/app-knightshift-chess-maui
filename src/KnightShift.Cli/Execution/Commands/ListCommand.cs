using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Cli.Rendering.State;

namespace KnightShift.Cli.Execution.Commands;

public class ListCommand : ICommand
{
    private readonly IGameService _game;

    public CommandInfo Info => new(
        Name: "list",
        Aliases: ["moves"],
        Parameter: "[square]",
        Description: "List legal moves",
        Category: "View",
        Order: 0
    );

    public ListCommand(IGameService game)
    {
        _game = game;
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

            string? square = commandParts.Length == 2 ? commandParts[1] : null;
            string squareSuffix = square is not null ? $" from {square}" : "";

            var movesCount = square is not null
                ? _game.GetLegalMoves(square).Count()
                : _game.GetLegalMoves().Count();
            
            var noun = (movesCount == 1) ? "move" : "moves";

            return Task.FromResult(new CommandResult
            {
                ContentType = UiContent.Moves,
                ContentState = new MovesContentState
                { 
                    OriginSquare = square
                },
                Message = movesCount == 0
                    ? $"Found no legal moves{squareSuffix}."
                    : $"Found {movesCount} legal {noun}{squareSuffix}."
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
