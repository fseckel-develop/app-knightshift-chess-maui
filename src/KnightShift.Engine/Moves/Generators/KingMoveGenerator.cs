using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;
using KnightShift.Domain.Constants;
using KnightShift.Engine.Moves.Helpers;

namespace KnightShift.Engine.Moves.Generators;

public class KingMoveGenerator : IPieceMoveGenerator
{
    public IEnumerable<Move> GenerateMoves(GameState state, Piece piece, Position from)
    {
        var moves = StepMoveGenerator.GenerateStepMoves(state, piece, from, Offsets.King).ToList();
        AddCastlingMoves(state, piece, from, moves);
        return moves;
    }

    private static void AddCastlingMoves(GameState state, Piece piece, Position from, List<Move> moves)
    {
        if (piece.Type != PieceType.King)
            return;

        TryAddCastle(state, from, moves, isKingSide: true, piece.Color);
        TryAddCastle(state, from, moves, isKingSide: false, piece.Color);
    }

    private static void TryAddCastle(GameState state, Position kingPosition, List<Move> moves, bool isKingSide, PieceColor color)
    {
        var board = state.Board;

        bool canCastle = color == PieceColor.White
            ? (isKingSide ? state.WhiteCanCastleKingSide : state.WhiteCanCastleQueenSide)
            : (isKingSide ? state.BlackCanCastleKingSide : state.BlackCanCastleQueenSide);

        if (!canCastle)
            return;
        
        int row = kingPosition.ToRow();

        var rookColumn = isKingSide ? 7 : 0;
        var rookPosition = Position.CreateFromCoords(row, rookColumn);
        var rook = board.GetPiece(rookPosition);

        if (rook is null || rook.Type != PieceType.Rook || rook.Color != color)
            return;

        if (isKingSide)
        {
            var f = Position.CreateFromCoords(row, 5);
            var g = Position.CreateFromCoords(row, 6);

            if (!board.IsEmpty(f) || !board.IsEmpty(g))
                return;

            moves.Add(new Move(kingPosition, g, IsCastling: true));
        }
        else
        {
            var d = Position.CreateFromCoords(row, 3);
            var c = Position.CreateFromCoords(row, 2);
            var b = Position.CreateFromCoords(row, 1);

            if (!board.IsEmpty(d) || !board.IsEmpty(c) || !board.IsEmpty(b))
                return;

            moves.Add(new Move(kingPosition, c, IsCastling: true));
        }
    }
}
