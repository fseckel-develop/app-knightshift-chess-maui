using KnightShift.Engine.Rules;
using KnightShift.Engine.Moves;
using KnightShift.Engine.Evaluation;

namespace KnightShift.Engine.Tests.Helpers;

public static class EngineTestFactory
{
    public static MoveValidator CreateMoveValidator()
    {
        var checkDetector = new CheckDetector();
        return new MoveValidator(checkDetector);
    }

    public static MoveGenerator CreateMoveGenerator()
    {
        var moveValidator = CreateMoveValidator();
        return new MoveGenerator(moveValidator);
    }

    public static GameResultEvaluator CreateEvaluator()
    {
        var checkDetector = new CheckDetector();
        var moveValidator = new MoveValidator(checkDetector);
        var moveGenerator = new MoveGenerator(moveValidator);
        return new GameResultEvaluator(moveGenerator, checkDetector);
    }
}
