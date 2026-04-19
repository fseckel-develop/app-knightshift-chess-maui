using KnightShift.Domain.Core;

namespace KnightShift.Application.Contracts.Interfaces;

public interface IGameStateSerializer
{
    GameState Deserialize(string input);
    bool TryDeserialize(string input, out GameState? state);
    string Serialize(GameState state);
}
