using KnightShift.Domain.Core;

namespace KnightShift.Engine.Moves.Generators;

public class KnightMoveGenerator : IPieceMoveGenerator
{
    private static readonly (int dRow, int dColumn)[] Offsets =
    [
        (-2, -1), (-2, 1), (-1, -2), (-1, 2), (1, -2), (1, 2), (2, -1), (2, 1)
    ];

    public IEnumerable<Move> GenerateMoves(GameState gameState, Piece piece, Position from)
    {
        var moves = new List<Move>();

        foreach (var (dRow, dColumn) in Offsets)
        {
            var row = from.ToRow() + dRow;
            var column = from.ToColumn() + dColumn;

            if (!Position.TryCreateFromCoords(row, column, out var targetPosition))
                continue;

            var targetPiece = gameState.Board.GetPiece(targetPosition);

            if (targetPiece == null || targetPiece.Color != piece.Color)
            {
                moves.Add(new Move(from, targetPosition));
            }
        }

        return moves;
    }
}
