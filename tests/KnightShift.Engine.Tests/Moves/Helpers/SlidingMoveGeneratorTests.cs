using FluentAssertions;
using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;
using KnightShift.Engine.Moves.Helpers;
using KnightShift.Engine.Tests.Helpers;

namespace KnightShift.Engine.Tests.Moves.Helpers;

public class SlidingMoveGeneratorTests
{
    [Fact]
    public void Should_Generate_Moves_Along_Direction_Until_Boundary()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.Rook, PieceColor.White, "d4")
            .Build();

        var directions = new[] { (-1, 0) };

        var moves = SlidingMoveGenerator.GenerateSlidingMoves(
            state,
            new Piece(PieceType.Rook, PieceColor.White),
            Position.CreateFromAlgebraic("d4"),
            directions);

        moves.Should().Contain(move => move.Target == Position.CreateFromAlgebraic("d5"));
        moves.Should().Contain(move => move.Target == Position.CreateFromAlgebraic("d8"));
    }

    [Fact]
    public void Should_Stop_At_Own_Piece_Without_Capturing()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.Rook, PieceColor.White, "d4")
            .WithPiece(PieceType.Pawn, PieceColor.White, "d6")
            .Build();

        var directions = new[] { (-1, 0) };

        var moves = SlidingMoveGenerator.GenerateSlidingMoves(
            state,
            new Piece(PieceType.Rook, PieceColor.White),
            Position.CreateFromAlgebraic("d4"),
            directions);

        moves.Should().NotContain(move => move.Target == Position.CreateFromAlgebraic("d6"));
        moves.Should().NotContain(move => move.Target == Position.CreateFromAlgebraic("d7"));
    }

    [Fact]
    public void Should_Capture_Enemy_Piece_And_Stop()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.Rook, PieceColor.White, "d4")
            .WithPiece(PieceType.Pawn, PieceColor.Black, "d6")
            .Build();

        var directions = new[] { (-1, 0) };

        var moves = SlidingMoveGenerator.GenerateSlidingMoves(
            state,
            new Piece(PieceType.Rook, PieceColor.White),
            Position.CreateFromAlgebraic("d4"),
            directions);

        moves.Should().Contain(move => move.Target == Position.CreateFromAlgebraic("d6"));
        moves.Should().NotContain(move => move.Target == Position.CreateFromAlgebraic("d7"));
    }

    [Fact]
    public void Should_Stop_At_Board_Boundary()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.Rook, PieceColor.White, "d8")
            .Build();

        var directions = new[] { (-1, 0) };

        var moves = SlidingMoveGenerator.GenerateSlidingMoves(
            state,
            new Piece(PieceType.Rook, PieceColor.White),
            Position.CreateFromAlgebraic("d8"),
            directions);

        moves.Should().BeEmpty();
    }

    [Fact]
    public void Should_Generate_Moves_In_Multiple_Directions()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.Rook, PieceColor.White, "d4")
            .Build();

        var directions = new[]
        {
            (1, 0), (-1, 0), (0, 1), (0, -1)
        };

        var moves = SlidingMoveGenerator.GenerateSlidingMoves(
            state,
            new Piece(PieceType.Rook, PieceColor.White),
            Position.CreateFromAlgebraic("d4"),
            directions);

        moves.Should().Contain(moves => moves.Target == Position.CreateFromAlgebraic("d8"));
        moves.Should().Contain(moves => moves.Target == Position.CreateFromAlgebraic("d1"));
        moves.Should().Contain(moves => moves.Target == Position.CreateFromAlgebraic("a4"));
        moves.Should().Contain(moves => moves.Target == Position.CreateFromAlgebraic("h4"));
    }
}
