using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;

namespace KnightShift.Engine.Moves.Generators;

public class PawnMoveGenerator : IPieceMoveGenerator
{
    public IEnumerable<Move> GenerateMoves(GameState state, Piece piece, Position from)
    {
        var moves = new List<Move>();
        var board = state.Board;

        int direction = piece.Color == PieceColor.White ? -1 : 1;
        int startRank = piece.Color == PieceColor.White ?  2 : 7;

        var row = from.ToRow();
        var column = from.ToColumn();

        // 1. Forward move
        var forwardRow = row + direction;
        if (IsInsideBoard(forwardRow, column))
        {
            var forwardPosition = Position.FromCoords(forwardRow, column);

            if (board.IsEmpty(forwardPosition))
            {
                moves.Add(new Move(from, forwardPosition));

                // 2. Double move
                if (from.Rank == startRank)
                {
                    var doubleRow = row + 2 * direction;
                    var doublePosition = Position.FromCoords(doubleRow, column);

                    if (board.IsEmpty(doublePosition))
                    {
                        moves.Add(new Move(from, doublePosition));
                    }
                }
            }
        }

        // 3. Captures
        var captureOffsets = new[] { -1, 1 };

        foreach (var offset in captureOffsets)
        {
            var captureColumn = column + offset;

            if (!IsInsideBoard(forwardRow, captureColumn))
                continue;

            var targetPosition = Position.FromCoords(forwardRow, captureColumn);
            var targetPiece = board.GetPiece(targetPosition);

            if (targetPiece != null && targetPiece.Color != piece.Color)
            {
                moves.Add(new Move(from, targetPosition));
            }
        }

        return moves;
    }

    private static bool IsInsideBoard(int row, int column)
        => row >= 0 && row < 8 && column >= 0 && column < 8;
}
