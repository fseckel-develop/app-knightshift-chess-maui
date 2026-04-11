using KnightShift.Domain.Core;
using KnightShift.Engine.Moves.Helpers;

namespace KnightShift.Engine.Moves.Generators;

public class KingMoveGenerator : IPieceMoveGenerator
{
    private static readonly (int dRow, int dColumn)[] Offsets =
    [
        (-1, 0), (1, 0), (0, -1), (0, 1), (-1, -1), (-1, 1), (1, -1), (1, 1)
    ];

    public IEnumerable<Move> GenerateMoves(GameState state, Piece piece, Position from)
    {
        return StepMoveGenerator.GenerateStepMoves(state, piece, from, Offsets);
    }
}
