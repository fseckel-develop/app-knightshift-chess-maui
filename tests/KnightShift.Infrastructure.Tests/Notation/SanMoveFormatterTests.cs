using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;
using KnightShift.Infrastructure.Tests.Helpers;

namespace KnightShift.Infrastructure.Tests.Notation;

public class SanMoveFormatterTests
{
    [Fact]
    public void Format_Should_Format_Pawn_Move()
    {
        var formatter = TestServices.Formatter();

        var state = TestGameStateFactory.CreateFromFen("4k3/8/8/8/8/8/4P3/4K3 w - -");

        var move = new Move(
            Position.CreateFromAlgebraic("e2"),
            Position.CreateFromAlgebraic("e4")
        );

        var result = formatter.Format(move, state, state.ApplyMove(move));

        Assert.Equal("e4", result);
    }

    [Fact]
    public void Format_Should_Format_Castling()
    {
        var formatter = TestServices.Formatter();

        var state = TestGameStateFactory.CreateFromFen("4k3/8/8/8/8/8/8/4K2R w K -");

        var move = new Move(
            Position.CreateFromAlgebraic("e1"),
            Position.CreateFromAlgebraic("g1"),
            IsCastling: true
        );

        var result = formatter.Format(move, state, state.ApplyMove(move));

        Assert.Equal("O-O", result);
    }

    [Fact]
    public void Format_Should_Format_Pawn_Capture()
    {
        var formatter = TestServices.Formatter();

        var state = TestGameStateFactory.CreateFromFen("4k3/8/3p4/4P3/8/8/8/4K3 w - -");

        var move = new Move(
            Position.CreateFromAlgebraic("e5"),
            Position.CreateFromAlgebraic("d6")
        );

        var result = formatter.Format(move, state, state.ApplyMove(move));

        Assert.Equal("exd6", result);
    }

    [Fact]
    public void Format_Should_Format_Promotion()
    {
        var formatter = TestServices.Formatter();

        var state = TestGameStateFactory.CreateFromFen("8/2P5/6k1/8/8/8/8/4K3 w - -");

        var move = new Move(
            Position.CreateFromAlgebraic("c7"),
            Position.CreateFromAlgebraic("c8"),
            Promotion: PieceType.Queen
        );

        var result = formatter.Format(move, state, state.ApplyMove(move));

        Assert.Equal("c8=Q", result);
    }

    [Fact]
    public void Format_Should_Append_Check_Symbol()
    {
        var formatter = TestServices.Formatter();

        var state = TestGameStateFactory.CreateFromFen("4k3/8/8/8/8/8/4Q3/4K3 w - -");

        var move = new Move(
            Position.CreateFromAlgebraic("e2"),
            Position.CreateFromAlgebraic("e7")
        );

        var result = formatter.Format(move, state, state.ApplyMove(move));

        Assert.EndsWith("+", result);
    }

    [Fact]
    public void Format_Should_Append_Checkmate_Symbol()
    {
        var formatter = TestServices.Formatter();

        var state = TestGameStateFactory.CreateFromFen("6k1/5Q2/6K1/8/8/8/8/8 w - -");

        var move = new Move(
            Position.CreateFromAlgebraic("f7"),
            Position.CreateFromAlgebraic("g7")
        );

        var result = formatter.Format(move, state, state.ApplyMove(move));

        Assert.EndsWith("#", result);
    }

    [Fact]
    public void Format_Should_Disambiguate_By_File()
    {
        var formatter = TestServices.Formatter();

        var state = TestGameStateFactory.CreateFromFen("4k3/8/8/8/3N1N2/8/8/4K3 w - -");

        var move = new Move(
            Position.CreateFromAlgebraic("d4"),
            Position.CreateFromAlgebraic("e6")
        );

        var result = formatter.Format(move, state, state.ApplyMove(move));

        Assert.StartsWith("N", result);
        Assert.Contains("d", result);
    }
}
