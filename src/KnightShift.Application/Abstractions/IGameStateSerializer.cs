using KnightShift.Domain.Core;

namespace KnightShift.Application.Abstractions;

public interface IGameStateSerializer
{
    GameState Deserialize(string input);
    string Serialize(GameState state);
}
