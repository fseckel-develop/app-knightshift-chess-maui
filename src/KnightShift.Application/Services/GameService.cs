using KnightShift.Application.Interfaces;
using KnightShift.Application.DTOs;
using KnightShift.Application.Mappers;
using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;
using KnightShift.Engine.Moves;
using KnightShift.Engine.Evaluation;

namespace KnightShift.Application.Services;

public class GameService : IGameService
{
    private readonly IMoveGenerator _moveGenerator;
    private readonly GameResultEvaluator _evaluator;
    private GameState _state;

    public GameService(IMoveGenerator moveGenerator, GameResultEvaluator evaluator)
    {
        _moveGenerator = moveGenerator;
        _evaluator = evaluator;

        _state = CreateInitialState();
    }

    public GameStateDto GetState()
        => GameStateMapper.ToDto(_state);

    public IEnumerable<MoveDto> GetLegalMoves()
    {
        return _moveGenerator
            .GenerateMoves(_state)
            .Select(MoveMapper.ToDto);
    }

    public IEnumerable<MoveDto> GetLegalMoves(string origin)
    {
        var position = Position.CreateFromAlgebraic(origin);
        return _moveGenerator
            .GenerateMoves(_state)
            .Where(move => move.Origin == position)
            .Select(MoveMapper.ToDto);
    }

    public void ApplyMove(MoveDto moveDto)
    {
        var move = MoveMapper.FromDto(moveDto);
        _state = _state.ApplyMove(move);
        _evaluator.Evaluate(_state);
    }

    public void StartNewGame()
    {
        _state = CreateInitialState();
    }

    private static GameState CreateInitialState()
    {
        var state = new GameState();

        // TODO: plug in proper initial setup

        return state;
    }

    public bool IsGameOver()
    {
        _evaluator.Evaluate(_state);
        return _state.Result != GameResult.Ongoing;
    }
}
