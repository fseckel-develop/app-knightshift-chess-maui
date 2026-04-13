using FluentAssertions;
using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;
using KnightShift.Engine.Moves.Generators;
using KnightShift.Engine.Tests.Helpers;

namespace KnightShift.Engine.Tests.Moves.Generators;

public class KingMoveGeneratorTests
{
    private readonly KingMoveGenerator _generator = new();

    [Fact]
    public void King_Should_Castle_KingSide_When_Allowed()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.King, PieceColor.White, "e1")
            .WithPiece(PieceType.Rook, PieceColor.White, "h1")
            .WithTurn(PieceColor.White)
            .WithCastlingRights(whiteKingSide: true)
            .Build();

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.King, PieceColor.White),
            Position.CreateFromAlgebraic("e1"));

        moves.Should().Contain(move => move.IsCastling && move.To == Position.CreateFromAlgebraic("g1"));
    }

    [Fact]
    public void King_Should_Not_Castle_KingSide_When_Path_Is_Blocked()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.King, PieceColor.White, "e1")
            .WithPiece(PieceType.Knight, PieceColor.White, "f1") // block
            .WithPiece(PieceType.Rook, PieceColor.White, "h1")
            .WithTurn(PieceColor.White)
            .WithCastlingRights(whiteKingSide: true)
            .Build();

        state.WhiteCanCastleKingSide = true;

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.King, PieceColor.White),
            Position.CreateFromAlgebraic("e1"));

        moves.Should().NotContain(move => move.IsCastling);
    }

    [Fact]
    public void King_Should_Not_Castle_KingSide_When_Rights_Are_Disabled()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.King, PieceColor.White, "e1")
            .WithPiece(PieceType.Rook, PieceColor.White, "h1")
            .WithTurn(PieceColor.White)
            .WithCastlingRights(whiteKingSide: false)
            .Build();

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.King, PieceColor.White),
            Position.CreateFromAlgebraic("e1"));

        moves.Should().NotContain(move => move.IsCastling);
    }

    [Fact]
    public void King_Should_Castle_QueenSide_When_Allowed()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.King, PieceColor.White, "e1")
            .WithPiece(PieceType.Rook, PieceColor.White, "a1")
            .WithTurn(PieceColor.White)
            .WithCastlingRights(whiteQueenSide: true)
            .Build();

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.King, PieceColor.White),
            Position.CreateFromAlgebraic("e1"));

        moves.Should().Contain(move => move.IsCastling && move.To == Position.CreateFromAlgebraic("c1"));
    }

    [Fact]
    public void King_Should_Not_Castle_QueenSide_When_Path_Is_Blocked()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.King, PieceColor.White, "e1")
            .WithPiece(PieceType.Knight, PieceColor.White, "d1") // block
            .WithPiece(PieceType.Rook, PieceColor.White, "a1")
            .WithTurn(PieceColor.White)
            .WithCastlingRights(whiteQueenSide: true)
            .Build();

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.King, PieceColor.White),
            Position.CreateFromAlgebraic("e1"));

        moves.Should().NotContain(move => move.IsCastling && move.To == Position.CreateFromAlgebraic("c1"));
    }

    [Fact]
    public void King_Should_Generate_Both_Castling_Moves_When_Both_Are_Available()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.King, PieceColor.White, "e1")
            .WithPiece(PieceType.Rook, PieceColor.White, "a1")
            .WithPiece(PieceType.Rook, PieceColor.White, "h1")
            .WithTurn(PieceColor.White)
            .WithCastlingRights(whiteKingSide: true, whiteQueenSide: true)
            .Build();

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.King, PieceColor.White),
            Position.CreateFromAlgebraic("e1"));

        moves.Should().Contain(move => move.To == Position.CreateFromAlgebraic("g1"));
        moves.Should().Contain(move => move.To == Position.CreateFromAlgebraic("c1"));
    }

    [Fact]
    public void Black_King_Should_Castle_KingSide_When_Allowed()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.King, PieceColor.Black, "e8")
            .WithPiece(PieceType.Rook, PieceColor.Black, "h8")
            .WithTurn(PieceColor.Black)
            .WithCastlingRights(blackKingSide: true)
            .Build();

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.King, PieceColor.Black),
            Position.CreateFromAlgebraic("e8"));

        moves.Should().Contain(move => move.IsCastling && move.To == Position.CreateFromAlgebraic("g8"));
    }

    [Fact]
    public void Black_King_Should_Castle_QueenSide_When_Allowed()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.King, PieceColor.Black, "e8")
            .WithPiece(PieceType.Rook, PieceColor.Black, "a8")
            .WithTurn(PieceColor.Black)
            .WithCastlingRights(blackQueenSide: true)
            .Build();

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.King, PieceColor.Black),
            Position.CreateFromAlgebraic("e8"));

        moves.Should().Contain(move => move.IsCastling && move.To == Position.CreateFromAlgebraic("c8"));
    }

    [Fact]
    public void King_Should_Not_Castle_When_Rook_Is_Missing()
    {
        var state = new TestGameStateBuilder()
            .WithPiece(PieceType.King, PieceColor.White, "e1")
            .WithTurn(PieceColor.White)
            .WithCastlingRights(whiteKingSide: true)
            .Build();

        var moves = _generator.GenerateMoves(
            state,
            new Piece(PieceType.King, PieceColor.White),
            Position.CreateFromAlgebraic("e1"));

        moves.Should().NotContain(move => move.IsCastling);
    }
}
