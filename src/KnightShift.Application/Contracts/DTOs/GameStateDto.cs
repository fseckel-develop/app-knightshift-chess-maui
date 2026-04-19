using KnightShift.Domain.Constants;

namespace KnightShift.Application.Contracts.DTOs;

public class GameStateDto
{
    public PieceDto?[,] Board { get; set; } = new PieceDto[BoardDimensions.Size, BoardDimensions.Size];

    public string CurrentTurn { get; set; } = default!;

    public MoveDto? LastMove { get; set; }

    public string Result { get; set; } = default!;

    public string EndReason { get; set; } = default!;
}
