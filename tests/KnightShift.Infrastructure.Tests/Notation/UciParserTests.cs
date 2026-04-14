using KnightShift.Infrastructure.Notation;
using KnightShift.Domain.Enums;

namespace KnightShift.Infrastructure.Tests.Notation;

public class UciParserTests
{
    [Fact]
    public void FromUci_Should_Parse_NormalMove()
    {
        var move = UciParser.FromUci("e2e4");

        Assert.Equal("e2", move.Origin.ToString());
        Assert.Equal("e4", move.Target.ToString());
        Assert.Null(move.Promotion);
    }

    [Theory]
    [InlineData("e7e8q", PieceType.Queen)]
    [InlineData("e7e8r", PieceType.Rook)]
    [InlineData("e7e8b", PieceType.Bishop)]
    [InlineData("e7e8n", PieceType.Knight)]
    public void FromUci_Should_Parse_All_Promotions(string uci, PieceType expected)
    {
        var move = UciParser.FromUci(uci);

        Assert.Equal(expected, move.Promotion);
    }

    [Fact]
    public void  ToUci_Should_Roundtrip_Promotion()
    {
        var move = UciParser.FromUci("e7e8q");

        var uci = UciParser.ToUci(move);

        Assert.Equal("e7e8q", uci);
    }
}
