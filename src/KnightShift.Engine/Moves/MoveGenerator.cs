using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;
using KnightShift.Engine.Rules;
using KnightShift.Engine.Moves.Generators;

namespace KnightShift.Engine.Moves;

public class MoveGenerator : IMoveGenerator
{
    private readonly MoveValidator _moveValidator;
    private readonly Dictionary<PieceType, IPieceMoveGenerator> _generators;

    public MoveGenerator(MoveValidator moveValidator)
    {
        _moveValidator = moveValidator;
        _generators = new ()
        {
            { PieceType.Pawn, new PawnMoveGenerator() },
            { PieceType.Knight, new KnightMoveGenerator() },
            { PieceType.Bishop, new BishopMoveGenerator() },
            { PieceType.Rook, new RookMoveGenerator() },
            { PieceType.Queen, new QueenMoveGenerator() },
            { PieceType.King, new KingMoveGenerator() }
        };
    }

    public IEnumerable<Move> GenerateMoves(GameState state)
    {
        var pseudeMoves = new List<Move>();

        foreach (var (position, piece) in state.Board.GetAllPieces())
        {
            if (piece.Color != state.CurrentTurn) 
                continue;

            var generator = _generators[piece.Type];
            pseudeMoves.AddRange(generator.GenerateMoves(state, piece, position));
        }
        var legalMoves = pseudeMoves.Where(move => _moveValidator.IsLegalMove(state, move)).ToList();

        return legalMoves;
    }
}
