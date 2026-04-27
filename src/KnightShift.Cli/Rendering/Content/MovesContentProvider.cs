using KnightShift.Application.Contracts.Interfaces;
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
        var contentState = state.ContentState as MovesContentState;

        var moves = contentState?.OriginSquare is not null
            ? _game.GetLegalMoves(contentState.OriginSquare).ToList()
            : _game.GetLegalMoves().ToList();

        if (moves.Count == 0)
            return [""];

        var content = new List<string>();

        const int columnWidth = 6;

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

            content.Add($"  {column1}  {column2}  {column3}");
        }

        return [.. content];
    }
}
