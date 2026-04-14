using KnightShift.Domain.Core;

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

        var direction = Pawn.GetForwardDirection(piece.Color);
        var (row, column) = Position.ToCoords(origin);
        var forwardRow = row + direction;

        if (!Position.TryCreateFromCoords(forwardRow, column, out var forwardPosition))
            return;

        if (!board.IsEmpty(forwardPosition))
            return;

        AddPromotionAwareMove(piece, origin, forwardPosition, moves);

        // Double move
        if (origin.Rank == Pawn.GetStartRank(piece.Color))
        {
            var doubleRow = forwardRow + direction;

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

        var direction = Pawn.GetForwardDirection(piece.Color);
        var (row, column) = Position.ToCoords(origin);

        foreach (var columnOffset in Pawn.CaptureOffsets)
        {
            var targetRow = row + direction;
            var targetColumn = column + columnOffset;

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
        if (target.Rank == Pawn.GetPromotionRank(piece.Color))
        {
            foreach (var promotion in Pawn.PromotionPieces)
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

        var direction = Pawn.GetForwardDirection(piece.Color);
        var (row, column) = Position.ToCoords(origin);
        var targetRow = row + direction;

        foreach (var columnOffset in Pawn.CaptureOffsets)
        {
            var targetColumn = column + columnOffset;

            if (!Position.TryCreateFromCoords(targetRow, targetColumn, out var targetPosition))
                continue;

            if (targetPosition == state.EnPassantTarget)
            {
                moves.Add(new Move(origin, targetPosition, IsEnPassant: true));
            }
        }
    }
}
