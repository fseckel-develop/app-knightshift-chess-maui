using KnightShift.Domain.Core;
using KnightShift.Domain.Exceptions;

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
    public void CreateFromCoords_Should_Return_Correct_Position()
    {
        var position = Position.CreateFromCoords(0, 0);

        Assert.Equal('a', position.File);
        Assert.Equal(8, position.Rank);
    }

    [Fact]
    public void CreateFromCoords_Invalid_Should_Throw()
    {
        Assert.Throws<InvalidPositionException>(() =>
            Position.CreateFromCoords(-1, 0));
    }

    [Fact]
    public void TryCreateFromCoords_Valid_Should_Return_True()
    {
        var result = Position.TryCreateFromCoords(0, 0, out var position);

        Assert.True(result);
        Assert.Equal('a', position.File);
        Assert.Equal(8, position.Rank);
    }

    [Fact]
    public void TryCreateFromCoords_Invalid_Should_Return_False()
    {
        var result = Position.TryCreateFromCoords(-1, 0, out _);

        Assert.False(result);
    }

    [Fact]
    public void CreateFromAlgebraic_Should_Parse_Correctly()
    {
        var position = Position.CreateFromAlgebraic("e4");

        Assert.Equal('e', position.File);
        Assert.Equal(4, position.Rank);
    }

    [Fact]
    public void CreateFromAlgebraic_Invalid_Should_Throw()
    {
        Assert.Throws<InvalidPositionException>(() =>
            Position.CreateFromAlgebraic("z9"));
    }

    [Fact]
    public void TryCreateFromAlgebraic_Invalid_Should_Return_False()
    {
        var result = Position.TryCreateFromAlgebraic("z9", out _);

        Assert.False(result);
    }
}
