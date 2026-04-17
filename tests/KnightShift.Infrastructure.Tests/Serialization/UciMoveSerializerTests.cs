using KnightShift.Infrastructure.Serialization;
using KnightShift.Domain.Enums;

namespace KnightShift.Infrastructure.Tests.Serialization;

public class UciMoveSerializerTests
{
    private readonly UciMoveSerializer _serializer = new();
    [Fact]
    public void Deserialize_Should_Parse_NormalMove()
    {
        var move = _serializer.Deserialize("e2e4");

        Assert.Equal("e2", move.Origin.ToString());
        Assert.Equal("e4", move.Target.ToString());
        Assert.Null(move.Promotion);
    }

    [Theory]
    [InlineData("e7e8q", PieceType.Queen)]
    [InlineData("e7e8r", PieceType.Rook)]
    [InlineData("e7e8b", PieceType.Bishop)]
    [InlineData("e7e8n", PieceType.Knight)]
    public void Deserialize_Should_Parse_All_Promotions(string uci, PieceType expected)
    {
        var move = _serializer.Deserialize(uci);

        Assert.Equal(expected, move.Promotion);
    }

    [Fact]
    public void  Serialize_Should_Roundtrip_Promotion()
    {
        var move = _serializer.Deserialize("e7e8q");

        var uci = _serializer.Serialize(move);

        Assert.Equal("e7e8q", uci);
    }
}
