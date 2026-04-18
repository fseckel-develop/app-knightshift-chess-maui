using KnightShift.Domain.Core;

namespace KnightShift.Application.Contracts.Interfaces;

public interface IGameStateSerializer
{
    GameState Deserialize(string input);
    string Serialize(GameState state);
}
