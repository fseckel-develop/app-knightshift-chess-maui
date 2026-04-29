using KnightShift.Infrastructure.Notation;
using KnightShift.Infrastructure.Serialization;
using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Engine.Moves;
using KnightShift.Engine.Rules;
using KnightShift.Engine.Evaluation;
using KnightShift.Infrastructure.Factories;

namespace KnightShift.Infrastructure.Tests.Helpers;

public static class TestServices
{
    public static IGameStateSerializer Serializer()
        => new FenGameStateSerializer();
    
    public static IGameStateFactory StateFactory()
        => new FenGameStateFactory(Serializer());

    public static ICheckDetector CheckDetector()
        => new CheckDetector();

    public static IMoveValidator MoveValidator()
        => new MoveValidator(CheckDetector());

    public static IMoveGenerator MoveGenerator()
        => new MoveGenerator(MoveValidator());

    public static IGameResultEvaluator Evaluator()
        => new GameResultEvaluator(
            MoveGenerator(),
            CheckDetector()
        );

    public static IMoveFormatter Formatter()
        => new SanMoveFormatter(
            Evaluator(),
            MoveGenerator()
        );

    public static SanMoveResolver Resolver()
        => new(MoveGenerator());
}
