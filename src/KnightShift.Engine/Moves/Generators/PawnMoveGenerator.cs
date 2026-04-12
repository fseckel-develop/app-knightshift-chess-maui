using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;

namespace KnightShift.Engine.Moves.Generators;

public class PawnMoveGenerator : IPieceMoveGenerator
{
    public IEnumerable<Move> GenerateMoves(GameState state, Piece piece, Position from)
    {
        var moves = new List<Move>();

        AddForwardMoves(state, piece, from, moves);
        AddCaptureMoves(state, piece, from, moves);
        AddEnPassantMoves(state, piece, from, moves);

        return moves;
    }

    private static void AddForwardMoves(GameState state, Piece piece, Position from, List<Move> moves)
    {
        var board = state.Board;

        int direction = piece.Color == PieceColor.White ? -1 : 1;
        int startRank = piece.Color == PieceColor.White ? 2 : 7;

        var row = from.ToRow();
        var column = from.ToColumn();

        var forwardRow = row + direction;

        if (!Position.TryCreateFromCoords(forwardRow, column, out var forwardPosition))
            return;

        if (!board.IsEmpty(forwardPosition))
            return;

        AddPromotionAwareMove(piece, from, forwardPosition, moves);

        // Double move
        if (from.Rank == startRank)
        {
            var doubleRow = row + 2 * direction;

            if (!Position.TryCreateFromCoords(doubleRow, column, out var doublePosition))
                return; 
                
            if (!board.IsEmpty(doublePosition))
                return;
            
            moves.Add(new Move(from, doublePosition));
        }
    }

    private static void AddCaptureMoves(GameState state, Piece piece, Position from, List<Move> moves)
    {
        var board = state.Board;

        int direction = piece.Color == PieceColor.White ? -1 : 1;

        var row = from.ToRow();
        var column = from.ToColumn();

        foreach (var offset in new[] { -1, 1 })
        {
            var targetRow = row + direction;
            var targetColumn = column + offset;

            if (!Position.TryCreateFromCoords(targetRow, targetColumn, out var targetPosition))
                continue;

            var targetPiece = board.GetPiece(targetPosition);

            if (targetPiece != null && targetPiece.Color != piece.Color)
            {
                AddPromotionAwareMove(piece, from, targetPosition, moves);
            }
        }
    }

    private static void AddPromotionAwareMove(Piece piece, Position from, Position to, List<Move> moves)
    {
        int promotionRank = piece.Color == PieceColor.White ? 8 : 1;

        if (to.Rank == promotionRank)
        {
            foreach (var promotion in new[]
            {
                PieceType.Queen,
                PieceType.Rook,
                PieceType.Bishop,
                PieceType.Knight
            })
            {
                moves.Add(new Move(from, to, Promotion: promotion));
            }
        }
        else
        {
            moves.Add(new Move(from, to));
        }
    }

    private static void AddEnPassantMoves(GameState state, Piece piece, Position from, List<Move> moves)
    {
        if (state.EnPassantTarget is null)
            return;

        int direction = piece.Color == PieceColor.White ? -1 : 1;

        var row = from.ToRow();
        var column = from.ToColumn();

        var targetRow = row + direction;

        foreach (var offset in new[] { -1, 1 })
        {
            var targetColumn = column + offset;

            if (!Position.TryCreateFromCoords(targetRow, targetColumn, out var targetPosition))
                continue;

            if (targetPosition == state.EnPassantTarget)
            {
                moves.Add(new Move(from, targetPosition, IsEnPassant: true));
            }
        }
    }
}
