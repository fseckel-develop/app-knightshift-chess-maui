using KnightShift.Infrastructure.Notation;
using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;

namespace KnightShift.Infrastructure.Tests.Notation;

public class FenParserTests
{
    [Fact]
    public void FromFen_Should_Parse_StartingPosition()
    {
        var fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq -";

        var state = FenParser.FromFen(fen);

        Assert.Equal(PieceColor.White, state.CurrentTurn);
        Assert.True(state.WhiteCanCastleKingSide);
        Assert.True(state.WhiteCanCastleQueenSide);
        Assert.True(state.BlackCanCastleKingSide);
        Assert.True(state.BlackCanCastleQueenSide);
        Assert.Null(state.EnPassantTarget);
    }

    [Fact]
    public void FromFen_Should_Parse_ComplexPosition()
    {
        var fen = "r1bqkbnr/pppp1ppp/2n5/4p3/3P4/2N5/PPP1PPPP/R1BQKBNR w KQkq -";

        var state = FenParser.FromFen(fen);

        // Spot checks
        Assert.NotNull(state.Board.GetPiece(Position.CreateFromAlgebraic("e5"))); // black pawn
        Assert.NotNull(state.Board.GetPiece(Position.CreateFromAlgebraic("d4"))); // white pawn
        Assert.NotNull(state.Board.GetPiece(Position.CreateFromAlgebraic("c3"))); // knight

        Assert.Equal(PieceColor.White, state.CurrentTurn);
    }

    [Fact]
    public void FromFen_Should_Throw_On_Invalid_RankCount()
    {
        var fen = "8/8/8/8/8/8/8 w - -"; // only 7 ranks

        Assert.Throws<ArgumentException>(() => FenParser.FromFen(fen));
    }
    
    [Fact]
    public void FromFen_Should_Throw_On_Invalid_Piece()
    {
        var fen = "8/8/8/8/8/8/8/X7 w - -";

        Assert.Throws<ArgumentException>(() => FenParser.FromFen(fen));
    }

    [Fact]
    public void ToFen_Should_Roundtrip_Correctly()
    {
        var fen = "8/8/8/8/8/8/8/8 w - -";

        var state = FenParser.FromFen(fen);
        var result = FenParser.ToFen(state);

        Assert.Equal(fen, result);
    }

    [Fact]
    public void FromFen_Should_Handle_NoCastlingRights()
    {
        var fen = "8/8/8/8/8/8/8/8 w - -";

        var state = FenParser.FromFen(fen);

        Assert.False(state.WhiteCanCastleKingSide);
        Assert.False(state.WhiteCanCastleQueenSide);
        Assert.False(state.BlackCanCastleKingSide);
        Assert.False(state.BlackCanCastleQueenSide);
    }

    [Fact]
    public void FromFen_Should_Parse_PartialCastlingRights()
    {
        var fen = "8/8/8/8/8/8/8/8 w Kq -";

        var state = FenParser.FromFen(fen);

        Assert.True(state.WhiteCanCastleKingSide);
        Assert.False(state.WhiteCanCastleQueenSide);
        Assert.False(state.BlackCanCastleKingSide);
        Assert.True(state.BlackCanCastleQueenSide);
    }

    [Fact]
    public void ToFen_Should_Preserve_CastlingRights()
    {
        var fen = "8/8/8/8/8/8/8/8 w Kq -";

        var state = FenParser.FromFen(fen);
        var result = FenParser.ToFen(state);

        Assert.Equal(fen, result);
    }

    [Fact]
    public void FromFen_Should_Parse_EnPassant_Target()
    {
        var fen = "8/8/8/3pP3/8/8/8/8 w - d6";

        var state = FenParser.FromFen(fen);

        Assert.NotNull(state.EnPassantTarget);
        Assert.Equal("d6", state.EnPassantTarget!.ToString());
    }

    [Fact]
    public void FromFen_Should_Handle_No_EnPassant()
    {
        var fen = "8/8/8/8/8/8/8/8 w - -";

        var state = FenParser.FromFen(fen);

        Assert.Null(state.EnPassantTarget);
    }

    [Fact]
    public void ToFen_Should_Preserve_EnPassant()
    {
        var fen = "8/8/8/3pP3/8/8/8/8 w - d6";

        var state = FenParser.FromFen(fen);
        var result = FenParser.ToFen(state);

        Assert.Equal(fen, result);
    }
}
