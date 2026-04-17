using KnightShift.Domain.Core;

namespace KnightShift.Application.Abstractions;

public interface IMoveSerializer
{
    Move Deserialize(string input);
    string Serialize(Move move);
}
