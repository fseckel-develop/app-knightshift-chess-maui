using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;
using KnightShift.Domain.Constants;
using KnightShift.Engine.Moves.Helpers;

namespace KnightShift.Engine.Moves.Generators;

public class KingMoveGenerator : IPieceMoveGenerator
{
    public IEnumerable<Move> GenerateMoves(GameState state, Piece piece, Position origin)
    {
        var moves = StepMoveGenerator.GenerateStepMoves(state, piece, origin, Offsets.King).ToList();
        AddCastlingMoves(state, piece, origin, moves);
        return moves;
    }

    private static void AddCastlingMoves(GameState state, Piece piece, Position from, List<Move> moves)
    {
        if (piece.Type != PieceType.King)
            return;

        TryAddCastle(state, from, moves, CastlingSide.KingSide, piece.Color);
        TryAddCastle(state, from, moves, CastlingSide.QueenSide, piece.Color);
    }

    private static void TryAddCastle(GameState state, Position kingPosition, List<Move> moves, CastlingSide side, PieceColor color)
    {
        var board = state.Board;

        bool canCastle = color == PieceColor.White
            ? (side == CastlingSide.KingSide ? state.WhiteCanCastleKingSide : state.WhiteCanCastleQueenSide)
            : (side == CastlingSide.KingSide ? state.BlackCanCastleKingSide : state.BlackCanCastleQueenSide);

        if (!canCastle)
            return;

        var rookPosition = Castling.GetRookOrigin(color, side);
        var rook = board.GetPiece(rookPosition);

        if (rook is null || rook.Type != PieceType.Rook || rook.Color != color)
            return;

        var pathSquares = Castling.GetPathSquares(color, side);

        if (pathSquares.Any(square => !board.IsEmpty(square)))
            return;

        var target = Castling.GetKingTarget(color, side);
        moves.Add(new Move(kingPosition, target, IsCastling: true));
    }
}
