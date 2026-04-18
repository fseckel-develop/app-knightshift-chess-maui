using KnightShift.Domain.Core;

namespace KnightShift.Application.Contracts.Interfaces;

public interface IMoveSerializer
{
    Move Deserialize(string input);
    string Serialize(Move move);
}
