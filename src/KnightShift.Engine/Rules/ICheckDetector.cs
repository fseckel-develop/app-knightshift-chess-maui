using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;

namespace KnightShift.Engine.Rules;

public interface ICheckDetector
{
    bool IsKingInCheck(GameState state, PieceColor color);
}
