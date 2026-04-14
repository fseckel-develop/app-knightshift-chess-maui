using KnightShift.Domain.Core;

namespace KnightShift.Engine.Moves.Helpers;

public static class SlidingMoveGenerator
{
    public static IEnumerable<Move> GenerateSlidingMoves(
        GameState state, Piece piece, Position origin,
        (int dRow, int dColumn)[] directions)
    {
        var moves = new List<Move>();
        var board = state.Board;

        foreach (var (dRow, dColumn) in directions)
        {
            int row = origin.ToRow();
            int column = origin.ToColumn();

            while (true)
            {
                row += dRow;
                column += dColumn;

                if (!Position.TryCreateFromCoords(row, column, out var targetPosition))
                    break;

                var targetPiece = board.GetPiece(targetPosition);

                if (targetPiece == null)
                {
                    moves.Add(new Move(origin, targetPosition));
                }
                else
                {
                    if (targetPiece.Color != piece.Color)
                    {
                        moves.Add(new Move(origin, targetPosition));
                    }
                    break;
                }
            }
        }

        return moves;
    }
}
