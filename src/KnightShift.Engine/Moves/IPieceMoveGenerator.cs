using KnightShift.Domain.Core;

namespace KnightShift.Engine.Moves;

public interface IPieceMoveGenerator
{
    IEnumerable<Move> GenerateMoves(GameState state, Piece piece, Position origin);
}
