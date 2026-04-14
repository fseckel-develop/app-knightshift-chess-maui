using KnightShift.Domain.Core;
using KnightShift.Domain.Constants;
using KnightShift.Engine.Moves.Helpers;

namespace KnightShift.Engine.Moves.Generators;

public class RookMoveGenerator : IPieceMoveGenerator
{
    public IEnumerable<Move> GenerateMoves(GameState state, Piece piece, Position origin)
    {
        return SlidingMoveGenerator.GenerateSlidingMoves(state, piece, origin, Directions.Rook);
    }
}
