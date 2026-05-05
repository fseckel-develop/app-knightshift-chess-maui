using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.Game.Models;
using KnightShift.Engine.Moves;
using KnightShift.Engine.Evaluation;

namespace KnightShift.Application.Game.Services;

public class GameStorageService
{
    private readonly IGameStateSerializer _serializer;
    private readonly IGameImporter _importer;
    private readonly IGameExporter _exporter;
    private readonly IMoveGenerator _generator;
    private readonly IGameResultEvaluator _evaluator;

    public GameStorageService(
        IGameStateSerializer serializer,
        IGameImporter importer,
        IGameExporter exporter,
        IMoveGenerator generator,
        IGameResultEvaluator evaluator)
    {
        _serializer = serializer;
        _importer = importer;
        _exporter = exporter;
        _generator = generator;
        _evaluator = evaluator;
    }

    public GameSession LoadState(string serializedState)
    {
        var game = new GameSession(_serializer.Deserialize(serializedState));
        _evaluator.Evaluate(game.CurrentState);
        return game;
    }

    public GameSession LoadGame(string serializedGame)
    {
        var (initialState, moves) = _importer.Import(serializedGame);
        var game = new GameSession(initialState);

        foreach (var move in moves)
        {
            var legalMoves = _generator.GenerateMoves(game.CurrentState);
            if (!legalMoves.Contains(move))
                throw new InvalidOperationException("Invalid move in imported game.");

            game.ApplyMove(move);
        }

        _evaluator.Evaluate(game.CurrentState);
        return game;
    }

    public string ExportState(GameSession game)
        => _serializer.Serialize(game.CurrentState);

    public string ExportGame(GameSession game)
    {
        var record = new GameRecord(game.InitialState, [..game.GetMoves()]);
        return _exporter.Export(record);
    }
}
