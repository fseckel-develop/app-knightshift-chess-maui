using KnightShift.Application.UseCases;
using KnightShift.Application.UseCases.GetHistory;
using KnightShift.Application.Game.Models;
using KnightShift.Cli.Rendering.State;

namespace KnightShift.Cli.Execution.Commands;

public class HistoryCommand : ICommand
{
    private readonly IQueryHandler<GetHistoryQuery, IEnumerable<MoveStep>> _handler;

    public CommandInfo Info => new(
        Name: "history",
        Aliases: ["san"],
        Parameter: null,
        Description: "Show move history (SAN)",
        Category: "View",
        Order: 3
    );

    public HistoryCommand(IQueryHandler<GetHistoryQuery, IEnumerable<MoveStep>> handler)
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
        var history = _handler.Handle(new GetHistoryQuery()).ToList();

        if (history.Count == 0)
        {
            return Task.FromResult(new CommandResult
            {
                Message = "No moves have been played yet."
            });
        }

        var noun = (history.Count == 1) ? "move" : "moves";

        return Task.FromResult(new CommandResult
        {
            ContentType = UiContent.History,
            Message = $"Tracked {history.Count} {noun} in this game."
        });
    }
}
