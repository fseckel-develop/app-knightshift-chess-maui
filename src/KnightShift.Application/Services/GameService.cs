using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.Contracts.DTOs;
using KnightShift.Application.Mappers;
using KnightShift.Application.Game;
using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;
using KnightShift.Engine.Moves;
using KnightShift.Engine.Evaluation;

namespace KnightShift.Application.Services;

public class GameService : IGameService
{
    private readonly IMoveGenerator _moveGenerator;
    private readonly IGameResultEvaluator _evaluator;
    private readonly IGameStateFactory _factory;
    private readonly IGameStateSerializer _stateSerializer;
    private readonly IMoveSerializer _moveSerializer;
    private readonly IGameExporter _exporter;
    private readonly IGameImporter _importer;

    private GameSession _game;

    public GameService(
        IMoveGenerator moveGenerator,
        IGameResultEvaluator evaluator,
        IGameStateFactory factory,
        IGameStateSerializer stateSerializer,
        IMoveSerializer moveSerializer,
        IGameExporter exporter,
        IGameImporter importer)
    {
        _moveGenerator = moveGenerator;
        _evaluator = evaluator;
        _factory = factory;
        _stateSerializer = stateSerializer;
        _moveSerializer = moveSerializer;
        _exporter = exporter;
        _importer = importer;

        _game = new GameSession(_factory.CreateInitialState());
        _evaluator.Evaluate(_game.CurrentState);
    }

    public GameStateDto GetState()
    {
        var state = GameStateMapper.ToDto(_game.CurrentState);
        state.CurrentIsInCheck = _evaluator.IsKingInCheck(_game.CurrentState);
        return state;
    }

    public IEnumerable<MoveDto> GetLegalMoves()
    {
        return _moveGenerator
            .GenerateMoves(_game.CurrentState)
            .Select(MoveMapper.ToDto);
    }

    public IEnumerable<MoveDto> GetLegalMoves(string origin)
    {
        var position = Position.CreateFromAlgebraic(origin);
        return _moveGenerator
            .GenerateMoves(_game.CurrentState)
            .Where(move => move.Origin == position)
            .Select(MoveMapper.ToDto);
    }

    public IEnumerable<MoveStep> GetHistory()
    {
        var state = _game.InitialState.Clone();

        foreach (var move in _game.GetMoves())
        {
            var stateBeforeMove = state;
            var stateAfterMove = state.ApplyMove(move);

            yield return new MoveStep(move, stateBeforeMove, stateAfterMove);

            state = stateAfterMove;
        }
    }

    public void ApplyMove(string serializedMove)
    {
        var requestedMove = _moveSerializer.Deserialize(serializedMove);
        var legalMoves = _moveGenerator.GenerateMoves(_game.CurrentState);
        
        var move = legalMoves.FirstOrDefault(move =>
            move.Origin == requestedMove.Origin &&
            move.Target == requestedMove.Target &&
            move.Promotion == requestedMove.Promotion
        ) 
        ?? throw new InvalidOperationException($"Move {serializedMove} is not legal.");

        _game.ApplyMove(move);
        _evaluator.Evaluate(_game.CurrentState);
    }

    public void UndoMove()
    {
        if (!_game.TryUndoMove())
            throw new InvalidOperationException("No move to undo.");
            
        _evaluator.Evaluate(_game.CurrentState);
    }

    public void RedoMove()
    {
        if (!_game.TryRedoMove())
            throw new InvalidOperationException("No move to redo.");

        _evaluator.Evaluate(_game.CurrentState);
    }

    public void StartNewGame()
    {
        _game = new GameSession(_factory.CreateInitialState());
        _evaluator.Evaluate(_game.CurrentState);
    }

    public void LoadState(string serializedState)
    {
        _game = new GameSession(_stateSerializer.Deserialize(serializedState));
        _evaluator.Evaluate(_game.CurrentState);
    }

    public void LoadGame(string serializedGame)
    {
        var (initialState, moves) = _importer.Import(serializedGame);
        _game = new GameSession(initialState);

        foreach (var move in moves)
        {
            var legalMoves = _moveGenerator.GenerateMoves(_game.CurrentState);
            if (!legalMoves.Contains(move))
                throw new InvalidOperationException("Invalid move in imported game.");

            _game.ApplyMove(move);
        }

        _evaluator.Evaluate(_game.CurrentState);
    }
    
    public string ExportState()
    {
        return _stateSerializer.Serialize(_game.CurrentState);
    }

    public string ExportGame()
    {
        var record = new GameRecord(_game.InitialState, [.._game.GetMoves()]);
        return _exporter.Export(record);
    }

    public bool IsGameOver()
    {
        _evaluator.Evaluate(_game.CurrentState);
        return _game.CurrentState.Result != GameResult.Ongoing;
    }
}
