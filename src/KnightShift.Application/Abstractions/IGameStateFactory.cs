using KnightShift.Domain.Core;

namespace KnightShift.Application.Abstractions;

public interface IGameStateFactory
{
    GameState CreateInitialState();
}
