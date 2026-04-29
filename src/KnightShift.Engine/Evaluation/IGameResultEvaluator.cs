using KnightShift.Domain.Core;

namespace KnightShift.Engine.Evaluation;

public interface IGameResultEvaluator
{
    void Evaluate(GameState state);
    bool IsKingInCheck(GameState state);
}
