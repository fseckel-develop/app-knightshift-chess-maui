using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Cli.Rendering.State;

namespace KnightShift.Cli.Rendering.Content;

public class HistoryContentProvider : IContentProvider
{
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
        var history = _game.GetHistory().ToList();

        if (history.Count == 0)
            return [""];

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

            content.Add($" {moveNumber,2}.  {whiteSan,-8} {blackSan,-8}");
        }

        return [.. content];
    }
}
