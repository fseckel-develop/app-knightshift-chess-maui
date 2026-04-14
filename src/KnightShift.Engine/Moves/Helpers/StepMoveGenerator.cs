using KnightShift.Domain.Core;

namespace KnightShift.Engine.Moves.Helpers;

public static class StepMoveGenerator
{
    public static IEnumerable<Move> GenerateStepMoves(
        GameState state, Piece piece, Position origin,
        (int dRow, int dColumn)[] offsets)
    {
        var moves = new List<Move>();
        var board = state.Board;
        var (originRow, originColumn) = Position.ToCoords(origin);

        foreach (var (dRow, dColumn) in offsets)
        {
            var row = originRow + dRow;
            var column = originColumn + dColumn;

            if (!Position.TryCreateFromCoords(row, column, out var targetPosition))
                continue;

            var targetPiece = board.GetPiece(targetPosition);

            if (targetPiece == null || targetPiece.Color != piece.Color)
            {
                moves.Add(new Move(origin, targetPosition));
            }
        }

        return moves;
    }
}
