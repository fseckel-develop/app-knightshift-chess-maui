using FluentAssertions;
using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;
using KnightShift.Engine.Rules;
using KnightShift.Engine.Tests.Helpers;

namespace KnightShift.Engine.Tests.Rules;

public class MoveValidatorTests
{
    private readonly MoveValidator _validator = new(new CheckDetector());

    [Fact]
    public void Should_Return_True_For_Legal_Move()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.King, PieceColor.White, "e1")
            .WithPiece(PieceType.Rook, PieceColor.Black, "e8")
            .Build();

        var move = new Move(
            Position.CreateFromAlgebraic("e1"),
            Position.CreateFromAlgebraic("f1"));

        var result = _validator.IsLegalMove(state, move);

        result.Should().BeTrue();
    }

    [Fact]
    public void Should_Return_False_When_Move_Leaves_King_In_Check()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.King, PieceColor.White, "e1")
            .WithPiece(PieceType.Rook, PieceColor.White, "e2") // shielding
            .WithPiece(PieceType.Rook, PieceColor.Black, "e8")
            .Build();

        var move = new Move(
            Position.CreateFromAlgebraic("e2"),
            Position.CreateFromAlgebraic("f2")); // exposes king

        var result = _validator.IsLegalMove(state, move);

        result.Should().BeFalse();
    }
}
