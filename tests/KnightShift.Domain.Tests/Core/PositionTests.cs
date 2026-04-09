using KnightShift.Domain.Core;

namespace KnightShift.Domain.Tests.Core;

public class PositionTests
{
    [Theory]
    [InlineData('a', 1)]
    [InlineData('h', 8)]
    [InlineData('e', 4)]
    public void Valid_Position_Should_Return_True(char file, int rank)
    {
        var position = new Position(file, rank);

        var result = position.IsValid();

        Assert.True(result);
    }

    [Theory]
    [InlineData('z', 1)]
    [InlineData('a', 9)]
    [InlineData('i', 0)]
    public void Invalid_Position_Should_Return_False(char file, int rank)
    {
        var position = new Position(file, rank);

        var result = position.IsValid();

        Assert.False(result);
    }

    [Fact]
    public void ToString_Should_Return_Chess_Notation()
    {
        var position = new Position('d', 5);

        var result = position.ToString();

        Assert.Equal("d5", result);
    }

    [Fact]
    public void ToRow_And_ToColumn_Should_Map_Correctly()
    {
        var position = new Position('a', 8);

        Assert.Equal(0, position.ToRow());
        Assert.Equal(0, position.ToColumn());
    }

    [Fact]
    public void FromCoords_Should_Return_Correct_Position()
    {
        var position = Position.FromCoords(0, 0);

        Assert.Equal('a', position.File);
        Assert.Equal(8, position.Rank);
    }
}
