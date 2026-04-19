namespace KnightShift.Application.Contracts.DTOs;

public class MoveDto
{
    public string Origin { get; set; } = default!;
    public string Target { get; set; } = default!;

    public int OriginRow { get; set; }
    public int OriginColumn { get; set; }

    public int TargetRow { get; set; }
    public int TargetColumn { get; set; }

    public string? Promotion { get; set; }

    public bool IsCastling { get; set; }
    public bool IsEnPassant { get; set; }
}
