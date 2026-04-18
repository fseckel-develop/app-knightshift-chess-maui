using KnightShift.Domain.Core;

namespace KnightShift.Application.Contracts.Interfaces;

public interface IGameStateFactory
{
    GameState CreateInitialState();
}
