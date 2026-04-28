using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Cli.Rendering.State;

namespace KnightShift.Cli.Rendering.Content;

public class HistoryContentProvider : IContentProvider
{
    private record HistoryEntry(
        int MoveNumber, 
        string WhiteSan, 
        string BlackSan
    );

    private readonly IGameService _game;
    private readonly IMoveFormatter _formatter;

    public UiContent ContentType => UiContent.History;

    public HistoryContentProvider(IGameService game, IMoveFormatter formatter)
    {
        _game = game;
        _formatter = formatter;
    }

    public string[] GetContent(UiState state)
    {
        return state.Mode switch
        {
            UiMode.Dashboard => GetDashboardHistory(),
            UiMode.Sequential => GetSequentialHistory(),
            _ => [""]
        };
    }

    private string[] GetDashboardHistory()
    {
        var historyEntries = BuildHistoryEntries();

        if (historyEntries.Count == 0)
            return [""];

        return [.. historyEntries
            .Select(entry => $" {entry.MoveNumber,2}.  {entry.WhiteSan,-8} {entry.BlackSan,-8}")
        ];
    }

    private string[] GetSequentialHistory()
    {
        var historyEntries = BuildHistoryEntries();

        if (historyEntries.Count == 0)
            return [""];

        var historyParts = historyEntries
            .Select(entry => $"{entry.MoveNumber}. {entry.WhiteSan} {entry.BlackSan}".Trim())
            .ToList();

        const int maxWidth = 80;
        var history = new List<string>();
        var currentLine = "";

        foreach (var historyPart in historyParts)
        {
            if ((currentLine + historyPart).Length > maxWidth)
            {
                history.Add("  " + currentLine.Trim());
                currentLine = "";
            }

            currentLine += historyPart + "  ";
        }

        if (!string.IsNullOrWhiteSpace(currentLine))
            history.Add("  " + currentLine.Trim());

        return [.. history];
    }

    private List<HistoryEntry> BuildHistoryEntries()
    {
        var history = _game.GetHistory().ToList();

        var result = new List<HistoryEntry>();

        for (int i = 0; i < history.Count; i += 2)
        {
            int moveNumber = i / 2 + 1;

            var whiteMoveStep = history[i];
            var whiteSan = _formatter.Format(
                whiteMoveStep.Move,
                whiteMoveStep.StateBeforeMove,
                whiteMoveStep.StateAfterMove
            );

            var blackStep = i + 1 < history.Count ? history[i + 1] : null;

            var blackMoveSan = blackStep is not null
                ? _formatter.Format(
                    blackStep.Move,
                    blackStep.StateBeforeMove,
                    blackStep.StateAfterMove
                )
                : "";

            result.Add(new HistoryEntry(moveNumber, whiteSan, blackMoveSan));
        }

        return result;
    }
}
