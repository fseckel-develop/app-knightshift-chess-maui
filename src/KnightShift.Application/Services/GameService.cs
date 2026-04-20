using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.Contracts.DTOs;
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
    private readonly IGameStateSerializer _stateSerializer;
    private readonly IMoveSerializer _moveSerializer;
    private GameState _state;

    public GameService(
        IMoveGenerator moveGenerator,
        GameResultEvaluator evaluator,
        IGameStateFactory factory,
        IGameStateSerializer stateSerializer,
        IMoveSerializer moveSerializer)
    {
        _moveGenerator = moveGenerator;
        _evaluator = evaluator;
        _factory = factory;
        _stateSerializer = stateSerializer;
        _moveSerializer = moveSerializer;

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

    public void ApplyMove(string serializedMove)
    {
        var parsed = _moveSerializer.Deserialize(serializedMove);
        var legalMoves = _moveGenerator.GenerateMoves(_state);
        
        var matched = legalMoves.FirstOrDefault(move =>
            move.Origin == parsed.Origin &&
            move.Target == parsed.Target &&
            move.Promotion == parsed.Promotion
        ) 
        ?? throw new InvalidOperationException("Illegal move.");

        _state = _state.ApplyMove(matched);
    }

    public void StartNewGame()
    {
        _state = _factory.CreateInitialState();
    }

    public void LoadState(string serializedState)
    {
        _state = _stateSerializer.Deserialize(serializedState);
        _evaluator.Evaluate(_state);
    }
    
    public string ExportState()
    {
        return _stateSerializer.Serialize(_state);
    }

    public bool IsGameOver()
    {
        _evaluator.Evaluate(_state);
        return _state.Result != GameResult.Ongoing;
    }
}
