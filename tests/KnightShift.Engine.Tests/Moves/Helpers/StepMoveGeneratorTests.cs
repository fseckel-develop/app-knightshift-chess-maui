using FluentAssertions;
using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;
using KnightShift.Engine.Moves.Helpers;
using KnightShift.Engine.Tests.Helpers;

namespace KnightShift.Engine.Tests.Moves.Helpers;

public class StepMoveGeneratorTests
{
    private static readonly (int, int)[] Offsets =
    [
        (-1, 0), (1, 0), (0, -1), (0, 1)
    ];

    [Fact]
    public void Should_Generate_Moves_To_Empty_Squares()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.King, PieceColor.White, "d4")
            .Build();

        var moves = StepMoveGenerator.GenerateStepMoves(
            state,
            new Piece(PieceType.King, PieceColor.White),
            Position.CreateFromAlgebraic("d4"),
            Offsets);

        moves.Should().Contain(move => move.To == Position.CreateFromAlgebraic("d5"));
    }

    [Fact]
    public void Should_Not_Generate_Move_Outside_Board()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.King, PieceColor.White, "a1")
            .Build();

        var moves = StepMoveGenerator.GenerateStepMoves(
            state,
            new Piece(PieceType.King, PieceColor.White),
            Position.CreateFromAlgebraic("a1"),
            Offsets);

        moves.Should().HaveCount(2);
    }

    [Fact]
    public void Should_Not_Capture_Own_Piece()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.King, PieceColor.White, "d4")
            .WithPiece(PieceType.Pawn, PieceColor.White, "d5")
            .Build();

        var moves = StepMoveGenerator.GenerateStepMoves(
            state,
            new Piece(PieceType.King, PieceColor.White),
            Position.CreateFromAlgebraic("d4"),
            Offsets);

        moves.Should().NotContain(move => move.To == Position.CreateFromAlgebraic("d5"));
    }

    [Fact]
    public void Should_Capture_Enemy_Piece()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.King, PieceColor.White, "d4")
            .WithPiece(PieceType.Pawn, PieceColor.Black, "d5")
            .Build();

        var moves = StepMoveGenerator.GenerateStepMoves(
            state,
            new Piece(PieceType.King, PieceColor.White),
            Position.CreateFromAlgebraic("d4"),
            Offsets);

        moves.Should().Contain(move => move.To == Position.CreateFromAlgebraic("d5"));
    }

    [Fact]
    public void Should_Have_No_Moves_When_All_Targets_Are_Own_Pieces()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.King, PieceColor.White, "d4")
            .WithPiece(PieceType.Pawn, PieceColor.White, "c4")
            .WithPiece(PieceType.Pawn, PieceColor.White, "e4")
            .WithPiece(PieceType.Pawn, PieceColor.White, "d3")
            .WithPiece(PieceType.Pawn, PieceColor.White, "d5")
            .WithTurn(PieceColor.White)
            .Build();

        var moves = StepMoveGenerator.GenerateStepMoves(
            state,
            new Piece(PieceType.King, PieceColor.White),
            Position.CreateFromAlgebraic("d4"),
            Offsets);

        moves.Should().BeEmpty();
    }
}
