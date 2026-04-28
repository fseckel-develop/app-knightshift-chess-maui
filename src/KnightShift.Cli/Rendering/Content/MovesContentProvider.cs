using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.Contracts.DTOs;
using KnightShift.Cli.Rendering.State;

namespace KnightShift.Cli.Rendering.Content;

public class MovesContentProvider : IContentProvider
{
    private readonly IGameService _game;

    public UiContent ContentType => UiContent.Moves;

    public MovesContentProvider(IGameService game)
    {
        _game = game;
    }

    public string[] GetContent(UiState state)
    {
        return state.Mode switch
        {
            UiMode.Dashboard => GetDashboardMoves(state),
            UiMode.Sequential => GetSequentialMoves(state),
            _ => [""]
        };
    }

    public string[] GetDashboardMoves(UiState state)
    {
        var moves = GetMoves(state);

        if (moves.Count == 0)
            return [""];

        const int columnWidth = 6;
        var lines = new List<string>();

        for (int i = 0; i < moves.Count; i += 3)
        {
            string Format(int index)
            {
                return index < moves.Count
                    ? moves[index].PadRight(columnWidth)
                    : "".PadRight(columnWidth);
            }

            lines.Add($"  {Format(i)}  {Format(i + 1)}  {Format(i + 2)}");
        }

        return [.. lines];
    }

    private string[] GetSequentialMoves(UiState state)
    {
        var moves = GetMoves(state);

        if (moves.Count == 0)
            return [""];

        const int movesPerLine = 8;
        var lines = new List<string>();

        for (int i = 0; i < moves.Count; i += movesPerLine)
        {
            var listChunk = moves.Skip(i).Take(movesPerLine);
            lines.Add("  " + string.Join(" ", listChunk));
        }

        return [.. lines];
    }

    private List<string> GetMoves(UiState state)
    {
        var contentState = state.ContentState as MovesContentState;

        var moves = contentState?.OriginSquare is not null
            ? _game.GetLegalMoves(contentState.OriginSquare).ToList()
            : _game.GetLegalMoves().ToList();
        
        return [.. moves.Select(move => $"{move.Origin}{move.Target}")];
    }
}
