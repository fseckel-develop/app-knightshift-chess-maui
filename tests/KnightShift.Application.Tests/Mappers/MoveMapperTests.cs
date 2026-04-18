using KnightShift.Application.Mappers;
using KnightShift.Application.Contracts.DTOs;
using KnightShift.Domain.Enums;

namespace KnightShift.Application.Tests.Mappers;

public class MoveMapperTests
{
    [Fact]
    public void ToDto_And_FromDto_ShouldRoundtrip_AllFields()
    {
        var dto = new MoveDto
        {
            Origin = "e2",
            Target = "e4",
            Promotion = "Queen",
            IsCastling = true,
            IsEnPassant = true
        };

        var move = MoveMapper.FromDto(dto);
        var result = MoveMapper.ToDto(move);

        Assert.Equal(dto.Origin, result.Origin);
        Assert.Equal(dto.Target, result.Target);
        Assert.Equal(dto.Promotion, result.Promotion);
        Assert.Equal(dto.IsCastling, result.IsCastling);
        Assert.Equal(dto.IsEnPassant, result.IsEnPassant);
    }

    [Fact]
    public void FromDto_ShouldParsePromotion()
    {
        var dto = new MoveDto
        {
            Origin = "e7",
            Target = "e8",
            Promotion = "Queen"
        };

        var move = MoveMapper.FromDto(dto);

        Assert.Equal(PieceType.Queen, move.Promotion);
    }

    [Fact]
    public void FromDto_ShouldThrow_OnInvalidPromotion()
    {
        var dto = new MoveDto
        {
            Origin = "e7",
            Target = "e8",
            Promotion = "InvalidPiece"
        };

        Assert.Throws<ArgumentException>(() => MoveMapper.FromDto(dto));
    }

    [Fact]
    public void FromDto_ShouldHandleNullPromotion()
    {
        var dto = new MoveDto
        {
            Origin = "e2",
            Target = "e4",
            Promotion = null
        };

        var move = MoveMapper.FromDto(dto);

        Assert.Null(move.Promotion);
    }

    [Fact]
    public void ToDto_ShouldConvertPromotionToString()
    {
        var dto = new MoveDto
        {
            Origin = "e7",
            Target = "e8",
            Promotion = "Knight"
        };

        var move = MoveMapper.FromDto(dto);
        var result = MoveMapper.ToDto(move);

        Assert.Equal("Knight", result.Promotion);
    }

    [Fact]
    public void FromDto_ShouldMapFlagsCorrectly()
    {
        var dto = new MoveDto
        {
            Origin = "e5",
            Target = "d6",
            IsEnPassant = true,
            IsCastling = false
        };

        var move = MoveMapper.FromDto(dto);

        Assert.True(move.IsEnPassant);
        Assert.False(move.IsCastling);
    }

    [Fact]
    public void ToDto_ShouldMapFlagsCorrectly()
    {
        var dto = new MoveDto
        {
            Origin = "e1",
            Target = "g1",
            IsCastling = true
        };

        var move = MoveMapper.FromDto(dto);
        var result = MoveMapper.ToDto(move);

        Assert.True(result.IsCastling);
    }
}
