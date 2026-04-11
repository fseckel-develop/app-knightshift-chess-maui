using KnightShift.Domain.Core;
using KnightShift.Engine.Moves.Helpers;

namespace KnightShift.Engine.Moves.Generators;

public class KnightMoveGenerator : IPieceMoveGenerator
{
    private static readonly (int dRow, int dColumn)[] Offsets =
    [
        (-2, -1), (-2, 1), (-1, -2), (-1, 2), (1, -2), (1, 2), (2, -1), (2, 1)
    ];

    public IEnumerable<Move> GenerateMoves(GameState state, Piece piece, Position from)
    {
        return StepMoveGenerator.GenerateStepMoves(state, piece, from, Offsets);
    }
}
