using FluentAssertions;
using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;
using KnightShift.Engine.Moves.Generators;
using KnightShift.Engine.Tests.Helpers;

namespace KnightShift.Engine.Tests.Moves.Generators;

public class KnightMoveGeneratorTests
{
    private readonly KnightMoveGenerator _generator = new();

    [Fact]
    public void Knight_Should_Generate_All_L_Shaped_Moves_From_Center()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.Knight, PieceColor.White, "d4")
            .WithTurn(PieceColor.White)
            .Build();

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.Knight, PieceColor.White),
            Position.CreateFromAlgebraic("d4"));

        moves.Should().HaveCount(8);
    }

    [Fact]
    public void Knight_Should_Handle_Board_Edges_Correctly()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.Knight, PieceColor.White, "a1")
            .WithTurn(PieceColor.White)
            .Build();

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.Knight, PieceColor.White),
            Position.CreateFromAlgebraic("a1"));

        moves.Should().HaveCount(2);
    }

    [Fact]
    public void Knight_Should_Use_L_Shaped_Movement_Correctly()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.Knight, PieceColor.White, "d4")
            .WithTurn(PieceColor.White)
            .Build();

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.Knight, PieceColor.White),
            Position.CreateFromAlgebraic("d4"));

        moves.Should().Contain(move => move.To == Position.CreateFromAlgebraic("e6"));
    }
}
