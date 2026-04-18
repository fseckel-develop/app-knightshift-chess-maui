namespace KnightShift.Application.Contracts.DTOs;

public class MoveDto
{
    public string Origin { get; set; } = default!;
    public string Target { get; set; } = default!;

    public string? Promotion { get; set; }

    public bool IsCastling { get; set; }
    public bool IsEnPassant { get; set; }
}
