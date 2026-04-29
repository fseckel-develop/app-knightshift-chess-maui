using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;
using KnightShift.Engine.Moves;
using KnightShift.Engine.Rules;

namespace KnightShift.Engine.Evaluation;

public class GameResultEvaluator : IGameResultEvaluator
{
    private readonly IMoveGenerator _moveGenerator;
    private readonly ICheckDetector _checkDetector;

    public GameResultEvaluator(IMoveGenerator moveGenerator, ICheckDetector checkDetector)
    {
        _moveGenerator = moveGenerator;
        _checkDetector = checkDetector;
    }

    public void Evaluate(GameState state)
    {
        var hasMoves = _moveGenerator.GenerateMoves(state).Any();

        if (hasMoves)
        {
            state.Result = GameResult.Ongoing;
            state.EndReason = GameEndReason.None;
            return;
        }

        if (IsKingInCheck(state))
        {
            state.Result = state.CurrentTurn == PieceColor.White
                ? GameResult.BlackWins
                : GameResult.WhiteWins;
            state.EndReason = GameEndReason.Checkmate;
        }
        else
        {
            state.Result = GameResult.Draw;
            state.EndReason = GameEndReason.Stalemate;
        }
    }

    public bool IsKingInCheck(GameState state)
    {
        return _checkDetector.IsKingInCheck(state, state.CurrentTurn);
    }
}
