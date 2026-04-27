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

            var moves = square is not null
                ? _game.GetLegalMoves(square).ToList()
                : _game.GetLegalMoves().ToList();
                        
            var noun = (moves.Count == 1) ? "move" : "moves";

            var content = new List<string>();

            const int columnWidth = 8;

            for (int i = 0; i < moves.Count; i += 3)
            {
                string Format(int index)
                {
                    if (index >= moves.Count)
                        return "".PadRight(columnWidth);

                    var move = moves[index];
                    return $"{move.Origin}{move.Target}".PadRight(columnWidth);
                }

                var column1 = Format(i);
                var column2 = Format(i + 1);
                var column3 = Format(i + 2);

                content.Add($" {column1} {column2} {column3}");
            }

            return Task.FromResult(new CommandResult
            {
                ContentType = UiContent.Moves,
                PanelContent = [.. content],
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
