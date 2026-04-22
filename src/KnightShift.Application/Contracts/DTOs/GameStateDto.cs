using KnightShift.Domain.Constants;

namespace KnightShift.Application.Contracts.DTOs;

public class GameStateDto
{
    public PieceDto?[,] Board { get; set; } = new PieceDto[BoardDimensions.Size, BoardDimensions.Size];
    public MoveDto? LastMove { get; set; }

    public PieceColorDto CurrentTurn { get; set; }
    public bool CurrentIsInCheck { get; set; }

    public GameResultDto GameResult { get; set; }
    public GameEndReasonDto GameEndReason { get; set; }
}
