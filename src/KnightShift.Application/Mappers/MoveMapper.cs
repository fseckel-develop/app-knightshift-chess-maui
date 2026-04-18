using KnightShift.Application.Contracts.DTOs;
using KnightShift.Domain.Core;
using KnightShift.Domain.Enums;

namespace KnightShift.Application.Mappers;

public static class MoveMapper
{
    public static MoveDto ToDto(Move move)
    {
        return new MoveDto
        {
            Origin = move.Origin.ToString(),
            Target = move.Target.ToString(),
            Promotion = move.Promotion?.ToString(),
            IsCastling = move.IsCastling,
            IsEnPassant = move.IsEnPassant
        };
    }

    public static Move FromDto(MoveDto dto)
    {
        return new Move(
            Position.CreateFromAlgebraic(dto.Origin),
            Position.CreateFromAlgebraic(dto.Target),
            dto.Promotion is null ? null : Enum.Parse<PieceType>(dto.Promotion),
            dto.IsCastling,
            dto.IsEnPassant
        );
    }
}
