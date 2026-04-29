using KnightShift.Domain.Core;

namespace KnightShift.Engine.Rules;

public class MoveValidator : IMoveValidator
{
    private readonly ICheckDetector _checkDetector;

    public MoveValidator(ICheckDetector checkDetector)
    {
        _checkDetector = checkDetector;
    }

    public bool IsLegalMove(GameState state, Move move)
    {
        var nextState = state.ApplyMove(move);
        return !_checkDetector.IsKingInCheck(nextState, state.CurrentTurn);
    }
}
