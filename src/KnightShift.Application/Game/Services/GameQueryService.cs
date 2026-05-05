using KnightShift.Application.Contracts.DTOs;
using KnightShift.Application.Mappers;
using KnightShift.Application.Game.Models;
using KnightShift.Engine.Moves;
using KnightShift.Engine.Evaluation;
using KnightShift.Domain.Enums;
using KnightShift.Domain.Core;

namespace KnightShift.Application.Game.Services;

public class GameQueryService
{
    private readonly IMoveGenerator _moveGenerator;
    private readonly IGameResultEvaluator _evaluator;

    public GameQueryService(
        IMoveGenerator moveGenerator,
        IGameResultEvaluator evaluator)
    {
        _moveGenerator = moveGenerator;
        _evaluator = evaluator;
    }

    public GameStateDto GetState(GameSession game)
    {
        var gameState = GameStateMapper.ToDto(game.CurrentState);
        gameState.CurrentIsInCheck = _evaluator.IsKingInCheck(game.CurrentState);
        return gameState;
    }

    public IEnumerable<MoveDto> GetLegalMoves(GameSession game)
    {
        return _moveGenerator
            .GenerateMoves(game.CurrentState)
            .Select(MoveMapper.ToDto);
    }

    public IEnumerable<MoveDto> GetLegalMoves(GameSession game, string origin)
    {
        var position = Position.CreateFromAlgebraic(origin);

        return _moveGenerator
            .GenerateMoves(game.CurrentState)
            .Where(move => move.Origin == position)
            .Select(MoveMapper.ToDto);
    }

    public IEnumerable<MoveStep> GetHistory(GameSession game)
    {
        var state = game.InitialState.Clone();

        foreach (var move in game.GetMoves())
        {
            var stateBeforeMove = state;
            var stateAfterMove = state.ApplyMove(move);

            yield return new MoveStep(move, stateBeforeMove, stateAfterMove);

            state = stateAfterMove;
        }
    }

    public bool IsGameOver(GameSession game)
    {
        _evaluator.Evaluate(game.CurrentState);
        return game.CurrentState.Result != GameResult.Ongoing;
    }
}
