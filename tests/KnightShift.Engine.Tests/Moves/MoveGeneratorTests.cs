using FluentAssertions;
using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;
using KnightShift.Engine.Moves;
using KnightShift.Engine.Tests.Helpers;

namespace KnightShift.Engine.Tests.Moves;

public class MoveGeneratorTests
{
    private readonly MoveGenerator _generator = EngineTestFactory.CreateMoveGenerator();

    [Fact]
    public void Should_Generate_Moves_Only_For_Current_Player()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.King, PieceColor.White, "e1")
            .WithPiece(PieceType.King, PieceColor.Black, "e8")
            .WithTurn(PieceColor.White)
            .Build();

        var moves = _generator.GenerateMoves(state);

        moves.Should().OnlyContain(move => state.Board.GetPiece(move.Origin)!.Color == PieceColor.White);
    }

    [Fact]
    public void Should_Filter_Out_Illegal_Moves()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.King, PieceColor.White, "e1")
            .WithPiece(PieceType.Rook, PieceColor.White, "e2")
            .WithPiece(PieceType.Rook, PieceColor.Black, "e8")
            .WithTurn(PieceColor.White)
            .Build();

        var moves = _generator.GenerateMoves(state);

        moves.Should().NotContain(move =>
            move.Origin == Position.CreateFromAlgebraic("e2") &&
            move.Target == Position.CreateFromAlgebraic("f2"));
    }

    [Fact]
    public void Should_Generate_Moves_For_All_Pieces()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.King, PieceColor.White, "e1")
            .WithPiece(PieceType.Knight, PieceColor.White, "d4")
            .WithTurn(PieceColor.White)
            .Build();

        var moves = _generator.GenerateMoves(state);

        moves.Should().Contain(move => move.Origin == Position.CreateFromAlgebraic("e1"));
        moves.Should().Contain(move => move.Origin == Position.CreateFromAlgebraic("d4"));
    }

    [Fact]
    public void Should_Return_Empty_When_No_Legal_Moves()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.King, PieceColor.White, "h1")
            .WithPiece(PieceType.Queen, PieceColor.Black, "g2")
            .WithPiece(PieceType.Rook, PieceColor.Black, "h2")
            .WithTurn(PieceColor.White)
            .Build();

        var moves = _generator.GenerateMoves(state);

        moves.Should().BeEmpty();
    }
}
