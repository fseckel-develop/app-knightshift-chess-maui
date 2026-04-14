using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;

namespace KnightShift.Engine.Moves.Generators;

public class PawnMoveGenerator : IPieceMoveGenerator
{
    public IEnumerable<Move> GenerateMoves(GameState state, Piece piece, Position origin)
    {
        var moves = new List<Move>();

        AddForwardMoves(state, piece, origin, moves);
        AddCaptureMoves(state, piece, origin, moves);
        AddEnPassantMoves(state, piece, origin, moves);

        return moves;
    }

    private static void AddForwardMoves(GameState state, Piece piece, Position origin, List<Move> moves)
    {
        var board = state.Board;

        int direction = piece.Color == PieceColor.White ? -1 : 1;
        int startRank = piece.Color == PieceColor.White ? 2 : 7;

        var row = origin.ToRow();
        var column = origin.ToColumn();

        var forwardRow = row + direction;

        if (!Position.TryCreateFromCoords(forwardRow, column, out var forwardPosition))
            return;

        if (!board.IsEmpty(forwardPosition))
            return;

        AddPromotionAwareMove(piece, origin, forwardPosition, moves);

        // Double move
        if (origin.Rank == startRank)
        {
            var doubleRow = row + 2 * direction;

            if (!Position.TryCreateFromCoords(doubleRow, column, out var doublePosition))
                return; 
                
            if (!board.IsEmpty(doublePosition))
                return;
            
            moves.Add(new Move(origin, doublePosition));
        }
    }

    private static void AddCaptureMoves(GameState state, Piece piece, Position origin, List<Move> moves)
    {
        var board = state.Board;

        int direction = piece.Color == PieceColor.White ? -1 : 1;

        var row = origin.ToRow();
        var column = origin.ToColumn();

        foreach (var offset in new[] { -1, 1 })
        {
            var targetRow = row + direction;
            var targetColumn = column + offset;

            if (!Position.TryCreateFromCoords(targetRow, targetColumn, out var targetPosition))
                continue;

            var targetPiece = board.GetPiece(targetPosition);

            if (targetPiece != null && targetPiece.Color != piece.Color)
            {
                AddPromotionAwareMove(piece, origin, targetPosition, moves);
            }
        }
    }

    private static void AddPromotionAwareMove(Piece piece, Position origin, Position target, List<Move> moves)
    {
        int promotionRank = piece.Color == PieceColor.White ? 8 : 1;

        if (target.Rank == promotionRank)
        {
            foreach (var promotion in new[]
            {
                PieceType.Queen,
                PieceType.Rook,
                PieceType.Bishop,
                PieceType.Knight
            })
            {
                moves.Add(new Move(origin, target, Promotion: promotion));
            }
        }
        else
        {
            moves.Add(new Move(origin, target));
        }
    }

    private static void AddEnPassantMoves(GameState state, Piece piece, Position origin, List<Move> moves)
    {
        if (state.EnPassantTarget is null)
            return;

        int direction = piece.Color == PieceColor.White ? -1 : 1;

        var row = origin.ToRow();
        var column = origin.ToColumn();

        var targetRow = row + direction;

        foreach (var offset in new[] { -1, 1 })
        {
            var targetColumn = column + offset;

            if (!Position.TryCreateFromCoords(targetRow, targetColumn, out var targetPosition))
                continue;

            if (targetPosition == state.EnPassantTarget)
            {
                moves.Add(new Move(origin, targetPosition, IsEnPassant: true));
            }
        }
    }
}
