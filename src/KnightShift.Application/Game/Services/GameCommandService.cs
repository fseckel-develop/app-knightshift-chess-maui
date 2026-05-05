using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.Game.Models;
using KnightShift.Engine.Moves;
using KnightShift.Engine.Evaluation;

namespace KnightShift.Application.Game.Services;

public class GameCommandService
{
    private readonly IMoveGenerator _moveGenerator;
    private readonly IMoveSerializer _moveSerializer;
    private readonly IGameResultEvaluator _evaluator;

    public GameCommandService(
        IMoveGenerator moveGenerator,
        IMoveSerializer moveSerializer,
        IGameResultEvaluator evaluator)
    {
        _moveGenerator = moveGenerator;
        _moveSerializer = moveSerializer;
        _evaluator = evaluator;
    }

    public void ApplyMove(GameSession game, string serializedMove)
    {
        var requestedMove = _moveSerializer.Deserialize(serializedMove);
        var legalMoves = _moveGenerator.GenerateMoves(game.CurrentState);

        var move = legalMoves.FirstOrDefault(move =>
            move.Origin == requestedMove.Origin &&
            move.Target == requestedMove.Target &&
            move.Promotion == requestedMove.Promotion
        ) ?? throw new InvalidOperationException($"Move {serializedMove} is not legal.");

        game.ApplyMove(move);
        _evaluator.Evaluate(game.CurrentState);
    }

    public void Undo(GameSession game)
    {
        if (!game.TryUndoMove())
            throw new InvalidOperationException("No move to undo.");

        _evaluator.Evaluate(game.CurrentState);
    }

    public void Redo(GameSession game)
    {
        if (!game.TryRedoMove())
            throw new InvalidOperationException("No move to redo.");

        _evaluator.Evaluate(game.CurrentState);
    }

    public GameSession StartNewGame(IGameStateFactory factory)
    {
        var game = new GameSession(factory.CreateInitialState());
        return game;
    }
}
