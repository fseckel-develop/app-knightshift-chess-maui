using KnightShift.Application.Abstractions;
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
    private readonly IGameStateFactory _factory;
    private GameState _state;

    public GameService(
        IMoveGenerator moveGenerator, 
        GameResultEvaluator evaluator, 
        IGameStateFactory factory)
    {
        _moveGenerator = moveGenerator;
        _evaluator = evaluator;
        _factory = factory;

        _state = _factory.CreateInitialState();
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
        _state = _factory.CreateInitialState();
    }

    public bool IsGameOver()
    {
        _evaluator.Evaluate(_state);
        return _state.Result != GameResult.Ongoing;
    }
}
