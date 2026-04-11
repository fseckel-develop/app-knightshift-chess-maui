using KnightShift.Domain.Core;

namespace KnightShift.Engine.Rules;

public class MoveValidator
{
    private readonly CheckDetector _checkDetector;

    public MoveValidator(CheckDetector checkDetector)
    {
        _checkDetector = checkDetector;
    }

    public bool IsLegalMove(GameState state, Move move)
    {
        var nextState = state.ApplyMove(move);
        return !_checkDetector.IsKingInCheck(nextState, state.CurrentTurn);
    }
}
