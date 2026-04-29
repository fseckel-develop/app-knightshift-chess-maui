using KnightShift.Domain.Core;

namespace KnightShift.Engine.Rules;

public interface IMoveValidator
{
    bool IsLegalMove(GameState state, Move move);
}
