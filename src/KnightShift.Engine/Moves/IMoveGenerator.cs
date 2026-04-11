using KnightShift.Domain.Core;

namespace KnightShift.Engine.Moves;

public interface IMoveGenerator
{
    IEnumerable<Move> GenerateMoves(GameState state);
}
