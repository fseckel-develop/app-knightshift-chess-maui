using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;
using KnightShift.Engine.Moves;
using KnightShift.Engine.Rules;

namespace KnightShift.Engine.Evaluation;

public class GameResultEvaluator
{
    private readonly IMoveGenerator _moveGenerator;
    private readonly CheckDetector _checkDetector;

    public GameResultEvaluator(IMoveGenerator moveGenerator, CheckDetector checkDetector)
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

        bool isInCheck = _checkDetector.IsKingInCheck(state, state.CurrentTurn);

        if (isInCheck)
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
}
