using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;
using KnightShift.Engine.Moves;
using KnightShift.Engine.Moves.Generators;

namespace KnightShift.Engine.Rules;

public class CheckDetector
{
    private readonly Dictionary<PieceType, IPieceMoveGenerator> _generators;

    public CheckDetector()
    {
        _generators = new()
        {
            { PieceType.Pawn, new PawnMoveGenerator() },
            { PieceType.Knight, new KnightMoveGenerator() },
            { PieceType.Bishop, new BishopMoveGenerator() },
            { PieceType.Rook, new RookMoveGenerator() },
            { PieceType.Queen, new QueenMoveGenerator() },
            { PieceType.King, new KingMoveGenerator() }
        };
    }

    public bool IsKingInCheck(GameState state, PieceColor color)
    {
        var kingPosition = FindKing(state, color);

        foreach (var (position, piece) in state.Board.GetAllPieces())
        {
            if (piece.Color == color)
                continue;

            var generator = _generators[piece.Type];
            var moves = generator.GenerateMoves(state, piece, position);

            if (moves.Any(move => move.Target == kingPosition))
                return true;
        }

        return false;
    }

    private static Position FindKing(GameState state, PieceColor color)
    {
        foreach (var (position, piece) in state.Board.GetAllPieces())
        {
            if (piece.Type == PieceType.King && piece.Color == color)
                return position;
        }

        throw new Exception("King not found");
    }
}
