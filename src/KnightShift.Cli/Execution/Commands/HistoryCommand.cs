using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Cli.Rendering.State;

namespace KnightShift.Cli.Execution.Commands;

public class HistoryCommand : ICommand
{
    private readonly IGameService _game;
    private readonly IMoveFormatter _formatter;

    public CommandInfo Info => new(
        Name: "history",
        Aliases: ["san"],
        Parameter: null,
        Description: "Show move history (SAN)",
        Category: "View",
        Order: 3
    );

    public HistoryCommand(IGameService game, IMoveFormatter formatter)
    {
        _game = game;
        _formatter = formatter;
    }

    public bool CanHandle(string input)
    {
        return input.Equals(Info.Name, StringComparison.OrdinalIgnoreCase) ||
            Info.Aliases.Any(alias => input.Equals(alias, StringComparison.OrdinalIgnoreCase));
    }

    public Task<CommandResult> ExecuteAsync(string input)
    {
        var history = _game.GetHistory().ToList();

        if (history.Count == 0)
        {
            return Task.FromResult(new CommandResult
            {
                Message = "No moves have been played yet."
            });
        }

        var noun = (history.Count == 1) ? "move" : "moves";

        var content = new List<string>();

        for (int i = 0; i < history.Count; i += 2)
        {
            int moveNumber = i / 2 + 1;

            var whiteMoveStep = history[i];
            var whiteSan = _formatter.Format(
                whiteMoveStep.Move,
                whiteMoveStep.StateBeforeMove,
                whiteMoveStep.StateAfterMove
            );

            var blackMoveStep = i + 1 < history.Count ? history[i + 1] : null;

            var blackSan = blackMoveStep is not null
                ? _formatter.Format(
                    blackMoveStep.Move,
                    blackMoveStep.StateBeforeMove,
                    blackMoveStep.StateAfterMove
                )
                : "";

            content.Add($"{moveNumber,2}. {whiteSan,-8} {blackSan,-8}");
        }

        return Task.FromResult(new CommandResult
        {
            ContentType = UiContent.History,
            PanelContent = [.. content],
            Message = $"Tracked {history.Count} {noun} in this game."
        });
    }
}
