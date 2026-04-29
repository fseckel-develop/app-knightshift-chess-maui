using KnightShift.Application.Contracts.DTOs;
using KnightShift.Application.Game;
using KnightShift.Domain.Core;

namespace KnightShift.Cli.Tests.Helpers;

public static class TestData
{
    public static MoveStep MoveStep()
    {
        return new MoveStep(
            CreateMove(),
            new GameState(),
            new GameState()
        );
    }

    public static MoveDto MoveDto(string from, string to)
        => new() { Origin = from, Target = to };

    public static MoveDto[] ManyMoveDtos(params (string from, string to)[] moves)
        => [.. moves.Select(move => MoveDto(move.from, move.to))];

    public static IEnumerable<MoveStep> History(int count)
        => [.. Enumerable.Range(0, count).Select(_ => MoveStep())];

    private static Move CreateMove()
    {
        var origin = Position.CreateFromAlgebraic("a2");
        var target = Position.CreateFromAlgebraic("a3");
        return new Move(origin, target); 
    }
}
