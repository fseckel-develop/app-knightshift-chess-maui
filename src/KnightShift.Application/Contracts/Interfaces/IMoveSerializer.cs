using KnightShift.Domain.Core;

namespace KnightShift.Application.Contracts.Interfaces;

public interface IMoveSerializer
{
    Move Deserialize(string input);
    bool TryDeserialize(string input, out Move? move);
    string Serialize(Move move);
}
