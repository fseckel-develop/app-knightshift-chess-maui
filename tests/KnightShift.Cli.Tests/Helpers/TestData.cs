using KnightShift.Application.Contracts.DTOs;
using KnightShift.Application.Game.Models;
using KnightShift.Domain.Core;

namespace KnightShift.Cli.Tests.Helpers;

public static class TestData
{
    public static MoveStep CreateMoveStep()
    {
        return new MoveStep(
            CreateMove(),
            new GameState(),
            new GameState()
        );
    }

    public static MoveDto CreateMoveDto(string from, string to)
        => new() { Origin = from, Target = to };

    public static MoveDto[] ManyMoveDtos(params (string from, string to)[] moves)
        => [.. moves.Select(move => CreateMoveDto(move.from, move.to))];

    public static IEnumerable<MoveStep> History(int count)
        => [.. Enumerable.Range(0, count).Select(_ => CreateMoveStep())];

    private static Move CreateMove()
    {
        var origin = Position.CreateFromAlgebraic("a2");
        var target = Position.CreateFromAlgebraic("a3");
        return new Move(origin, target); 
    }
}
