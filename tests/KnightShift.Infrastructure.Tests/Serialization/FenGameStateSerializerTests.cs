using KnightShift.Infrastructure.Serialization;
using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;

namespace KnightShift.Infrastructure.Tests.Serialization;

public class FenGameStateSerializerTests
{
    private readonly FenGameStateSerializer _serialzer = new();

    [Fact]
    public void Deserialize_Should_Parse_StartingPosition()
    {
        var fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq -";

        var state = _serialzer.Deserialize(fen);

        Assert.Equal(PieceColor.White, state.CurrentTurn);
        Assert.True(state.WhiteCanCastleKingSide);
        Assert.True(state.WhiteCanCastleQueenSide);
        Assert.True(state.BlackCanCastleKingSide);
        Assert.True(state.BlackCanCastleQueenSide);
        Assert.Null(state.EnPassantTarget);
    }

    [Fact]
    public void Deserialize_Should_Parse_ComplexPosition()
    {
        var fen = "r1bqkbnr/pppp1ppp/2n5/4p3/3P4/2N5/PPP1PPPP/R1BQKBNR w KQkq -";

        var state = _serialzer.Deserialize(fen);

        // Spot checks
        Assert.NotNull(state.Board.GetPiece(Position.CreateFromAlgebraic("e5"))); // black pawn
        Assert.NotNull(state.Board.GetPiece(Position.CreateFromAlgebraic("d4"))); // white pawn
        Assert.NotNull(state.Board.GetPiece(Position.CreateFromAlgebraic("c3"))); // knight

        Assert.Equal(PieceColor.White, state.CurrentTurn);
    }

    [Fact]
    public void Deserialize_Should_Throw_On_Invalid_RankCount()
    {
        var fen = "8/8/8/8/8/8/8 w - -"; // only 7 ranks

        Assert.Throws<ArgumentException>(() => _serialzer.Deserialize(fen));
    }
    
    [Fact]
    public void Deserialize_Should_Throw_On_Invalid_Piece()
    {
        var fen = "8/8/8/8/8/8/8/X7 w - -";

        Assert.Throws<ArgumentException>(() => _serialzer.Deserialize(fen));
    }

    [Fact]
    public void Serialize_Should_Roundtrip_Correctly()
    {
        var fen = "8/8/8/8/8/8/8/8 w - -";

        var state = _serialzer.Deserialize(fen);
        var result = _serialzer.Serialize(state);

        Assert.Equal(fen, result);
    }

    [Fact]
    public void Deserialize_Should_Handle_NoCastlingRights()
    {
        var fen = "8/8/8/8/8/8/8/8 w - -";

        var state = _serialzer.Deserialize(fen);

        Assert.False(state.WhiteCanCastleKingSide);
        Assert.False(state.WhiteCanCastleQueenSide);
        Assert.False(state.BlackCanCastleKingSide);
        Assert.False(state.BlackCanCastleQueenSide);
    }

    [Fact]
    public void Deserialize_Should_Parse_PartialCastlingRights()
    {
        var fen = "8/8/8/8/8/8/8/8 w Kq -";

        var state = _serialzer.Deserialize(fen);

        Assert.True(state.WhiteCanCastleKingSide);
        Assert.False(state.WhiteCanCastleQueenSide);
        Assert.False(state.BlackCanCastleKingSide);
        Assert.True(state.BlackCanCastleQueenSide);
    }

    [Fact]
    public void Serialize_Should_Preserve_CastlingRights()
    {
        var fen = "8/8/8/8/8/8/8/8 w Kq -";

        var state = _serialzer.Deserialize(fen);
        var result = _serialzer.Serialize(state);

        Assert.Equal(fen, result);
    }

    [Fact]
    public void Deserialize_Should_Parse_EnPassant_Target()
    {
        var fen = "8/8/8/3pP3/8/8/8/8 w - d6";

        var state = _serialzer.Deserialize(fen);

        Assert.NotNull(state.EnPassantTarget);
        Assert.Equal("d6", state.EnPassantTarget!.ToString());
    }

    [Fact]
    public void Deserialize_Should_Handle_No_EnPassant()
    {
        var fen = "8/8/8/8/8/8/8/8 w - -";

        var state = _serialzer.Deserialize(fen);

        Assert.Null(state.EnPassantTarget);
    }

    [Fact]
    public void Serialize_Should_Preserve_EnPassant()
    {
        var fen = "8/8/8/3pP3/8/8/8/8 w - d6";

        var state = _serialzer.Deserialize(fen);
        var result = _serialzer.Serialize(state);

        Assert.Equal(fen, result);
    }
}
