using KnightShift.Domain.Enums;

namespace KnightShift.Application.Contracts.DTOs;

public class PieceDto
{
    public PieceType Type { get; set; }
    public PieceColor Color { get; set; }
}
