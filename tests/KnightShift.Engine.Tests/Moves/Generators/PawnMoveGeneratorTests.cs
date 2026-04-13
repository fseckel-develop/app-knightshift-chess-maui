using FluentAssertions;
using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;
using KnightShift.Engine.Moves.Generators;
using KnightShift.Engine.Tests.Helpers;

namespace KnightShift.Engine.Tests.Moves.Generators;

public class PawnMoveGeneratorTests
{
    private readonly PawnMoveGenerator _generator = new();

    [Fact]
    public void Pawn_Should_Move_One_Forward()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.Pawn, PieceColor.White, "e2")
            .WithTurn(PieceColor.White)
            .Build();

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.Pawn, PieceColor.White),
            Position.CreateFromAlgebraic("e2"));

        moves.Should().Contain(move => move.To == Position.CreateFromAlgebraic("e3"));
    }

    [Fact]
    public void Pawn_Should_Move_Two_From_Start()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.Pawn, PieceColor.White, "e2")
            .WithTurn(PieceColor.White)
            .Build();

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.Pawn, PieceColor.White),
            Position.CreateFromAlgebraic("e2"));

        moves.Should().Contain(move => move.To == Position.CreateFromAlgebraic("e4"));
    }

    [Fact]
    public void Pawn_Should_Not_Double_Move_If_Second_Square_Is_Blocked()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.Pawn, PieceColor.White, "e2")
            .WithPiece(PieceType.Knight, PieceColor.Black, "e4")
            .WithTurn(PieceColor.White)
            .Build();

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.Pawn, PieceColor.White),
            Position.CreateFromAlgebraic("e2"));

        moves.Should().NotContain(move => move.To == Position.CreateFromAlgebraic("e4"));
    }

    [Fact]
    public void Pawn_Should_Not_Double_Move_If_Not_On_Start_Rank()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.Pawn, PieceColor.White, "e3")
            .WithTurn(PieceColor.White)
            .Build();

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.Pawn, PieceColor.White),
            Position.CreateFromAlgebraic("e3"));

        moves.Should().NotContain(move => move.To == Position.CreateFromAlgebraic("e5"));
    }

    [Fact]
    public void Pawn_Should_Not_Jump_Over_Piece()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.Pawn, PieceColor.White, "e2")
            .WithPiece(PieceType.Knight, PieceColor.White, "e3")
            .WithTurn(PieceColor.White)
            .Build();

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.Pawn, PieceColor.White),
            Position.CreateFromAlgebraic("e2"));

        moves.Should().BeEmpty();
    }

    [Fact]
    public void Pawn_Should_Capture_Diagonally()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.Pawn, PieceColor.White, "e4")
            .WithPiece(PieceType.Knight, PieceColor.Black, "d5")
            .WithTurn(PieceColor.White)
            .Build();

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.Pawn, PieceColor.White),
            Position.CreateFromAlgebraic("e4"));

        moves.Should().Contain(move => move.To == Position.CreateFromAlgebraic("d5"));
    }

    [Fact]
    public void Pawn_Should_Not_Capture_Own_Piece()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.Pawn, PieceColor.White, "e4")
            .WithPiece(PieceType.Knight, PieceColor.White, "d5")
            .WithTurn(PieceColor.White)
            .Build();

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.Pawn, PieceColor.White),
            Position.CreateFromAlgebraic("e4"));

        moves.Should().NotContain(move => move.To == Position.CreateFromAlgebraic("d5"));
    }

    [Fact]
    public void Pawn_Should_Not_Capture_If_No_Piece_Present()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.Pawn, PieceColor.White, "e4")
            .WithTurn(PieceColor.White)
            .Build();

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.Pawn, PieceColor.White),
            Position.CreateFromAlgebraic("e4"));

        moves.Should().NotContain(move => move.To == Position.CreateFromAlgebraic("d5"));
    }

    [Fact]
    public void Pawn_Should_Not_Move_Forward_If_Blocked_But_Can_Capture()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.Pawn, PieceColor.White, "e4")
            .WithPiece(PieceType.Pawn, PieceColor.White, "e5") // block
            .WithPiece(PieceType.Knight, PieceColor.Black, "d5")
            .WithTurn(PieceColor.White)
            .Build();

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.Pawn, PieceColor.White),
            Position.CreateFromAlgebraic("e4"));

        moves.Should().NotContain(move => move.To == Position.CreateFromAlgebraic("e5"));
        moves.Should().Contain(move => move.To == Position.CreateFromAlgebraic("d5"));
    }

    [Fact]
    public void Pawn_Should_Generate_Promotion_Moves()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.Pawn, PieceColor.White, "e7")
            .WithTurn(PieceColor.White)
            .Build();

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.Pawn, PieceColor.White),
            Position.CreateFromAlgebraic("e7"));

        moves.Count(move => move.To == Position.CreateFromAlgebraic("e8")).Should().Be(4);
    }

    [Fact]
    public void Pawn_Should_Promote_On_Capture()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.Pawn, PieceColor.White, "e7")
            .WithPiece(PieceType.Rook, PieceColor.Black, "d8")
            .WithTurn(PieceColor.White)
            .Build();

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.Pawn, PieceColor.White),
            Position.CreateFromAlgebraic("e7"));

        moves.Count(move => move.To == Position.CreateFromAlgebraic("d8")).Should().Be(4);
    }

    [Fact]
    public void Pawn_Should_Not_Promote_If_Not_On_Last_Rank()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.Pawn, PieceColor.White, "e6")
            .WithTurn(PieceColor.White)
            .Build();

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.Pawn, PieceColor.White),
            Position.CreateFromAlgebraic("e6"));

        moves.Count(move => move.Promotion != null).Should().Be(0);
    }

    [Fact]
    public void Pawn_Should_Generate_EnPassant()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.Pawn, PieceColor.White, "e5")
            .WithEnPassant("d6")
            .WithTurn(PieceColor.White)
            .Build();

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.Pawn, PieceColor.White),
            Position.CreateFromAlgebraic("e5"));

        moves.Should().Contain(move => move.IsEnPassant);
    }

    [Fact]
    public void Pawn_Should_Not_Generate_EnPassant_If_Target_Does_Not_Match()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.Pawn, PieceColor.White, "e5")
            .WithEnPassant("c6") // irrelevant
            .WithTurn(PieceColor.White)
            .Build();

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.Pawn, PieceColor.White),
            Position.CreateFromAlgebraic("e5"));

        moves.Should().NotContain(move => move.IsEnPassant);
    }

    [Fact]
    public void Pawn_Should_Not_Generate_EnPassant_When_No_Target()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.Pawn, PieceColor.White, "e5")
            .WithTurn(PieceColor.White)
            .Build();

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.Pawn, PieceColor.White),
            Position.CreateFromAlgebraic("e5"));

        moves.Should().NotContain(move => move.IsEnPassant);
    }
}
