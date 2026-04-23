using KnightShift.Domain.Core;

namespace KnightShift.Application.Contracts.Interfaces;

public interface IMoveFormatter
{
    string Format(Move move, GameState stateBeforeMove, GameState stateAfterMove);
}
