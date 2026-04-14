using KnightShift.Domain.Core;
using KnightShift.Domain.Constants;
using KnightShift.Engine.Moves.Helpers;

namespace KnightShift.Engine.Moves.Generators;

public class KnightMoveGenerator : IPieceMoveGenerator
{
    public IEnumerable<Move> GenerateMoves(GameState state, Piece piece, Position from)
    {
        return StepMoveGenerator.GenerateStepMoves(state, piece, from, Offsets.Knight);
    }
}
