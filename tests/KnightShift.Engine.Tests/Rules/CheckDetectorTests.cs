using FluentAssertions;
using KnightShift.Domain.Enums;
using KnightShift.Engine.Rules;
using KnightShift.Engine.Tests.Helpers;

namespace KnightShift.Engine.Tests.Rules;

public class CheckDetectorTests
{
    private readonly CheckDetector _detector = new();

    [Fact]
    public void Should_Return_True_When_King_Is_In_Check_By_Rook()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.King, PieceColor.White, "e1")
            .WithPiece(PieceType.Rook, PieceColor.Black, "e8")
            .Build();

        var result = _detector.IsKingInCheck(state, PieceColor.White);

        result.Should().BeTrue();
    }

    [Fact]
    public void Should_Return_False_When_King_Is_Not_In_Check()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.King, PieceColor.White, "e1")
            .WithPiece(PieceType.Rook, PieceColor.Black, "a8")
            .Build();

        var result = _detector.IsKingInCheck(state, PieceColor.White);

        result.Should().BeFalse();
    }

    [Fact]
    public void Should_Detect_Check_From_Knight()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.King, PieceColor.White, "e4")
            .WithPiece(PieceType.Knight, PieceColor.Black, "d6")
            .Build();

        var result = _detector.IsKingInCheck(state, PieceColor.White);

        result.Should().BeTrue();
    }

    [Fact]
    public void Should_Throw_When_King_Not_Found()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.Rook, PieceColor.Black, "e8")
            .Build();

        var act = () => _detector.IsKingInCheck(state, PieceColor.White);

        act.Should().Throw<Exception>();
    }
}
