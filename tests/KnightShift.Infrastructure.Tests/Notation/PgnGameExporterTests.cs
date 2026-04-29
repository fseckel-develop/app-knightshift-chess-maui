using KnightShift.Infrastructure.Notation;
using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.Game;
using KnightShift.Domain.Core;
using KnightShift.Infrastructure.Tests.Helpers;

namespace KnightShift.Infrastructure.Tests.Notation;

public class PgnGameExporterTests
{
    private class FakeFormatter : IMoveFormatter
    {
        public string Format(Move move, GameState before, GameState after)
            => $"{move.Origin}{move.Target}";
    }

    [Fact]
    public void Export_Should_Generate_Pgn_With_Moves_And_Metadata()
    {
        var exporter = new PgnGameExporter(new FakeFormatter());

        var initial = TestGameStateFactory.CreateInitial();

        var moves = new List<Move>
        {
            new(Position.CreateFromAlgebraic("e2"), Position.CreateFromAlgebraic("e4")),
            new(Position.CreateFromAlgebraic("e7"), Position.CreateFromAlgebraic("e5"))
        };

        var record = new GameRecord(initial, moves);

        var result = exporter.Export(record);

        Assert.Contains("[Event", result);
        Assert.Contains("[Result", result);
        Assert.Contains("1. e2e4 e7e5", result);
    }

    [Fact]
    public void Export_Should_Handle_Empty_Moves()
    {
        var exporter = new PgnGameExporter(new FakeFormatter());
        var state = TestGameStateFactory.CreateInitial();

        var record = new GameRecord(state, []);

        var result = exporter.Export(record);

        Assert.Contains("[Result", result);
    }

    [Fact]
    public void Export_Should_Number_Moves_Correctly()
    {
        var exporter = new PgnGameExporter(new FakeFormatter());

        var state = TestGameStateFactory.CreateInitial();

        var moves = new List<Move>
        {
            new(Position.CreateFromAlgebraic("e2"), Position.CreateFromAlgebraic("e4")),
            new(Position.CreateFromAlgebraic("e7"), Position.CreateFromAlgebraic("e5")),
            new(Position.CreateFromAlgebraic("g1"), Position.CreateFromAlgebraic("f3"))
        };

        var record = new GameRecord(state, moves);

        var result = exporter.Export(record);

        Assert.Contains("1. e2e4 e7e5 2. g1f3", result);
    }
}
