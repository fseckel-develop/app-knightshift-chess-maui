namespace KnightShift.Application.DTOs;

public class GameStateDto
{
    public string[][] Board { get; set; } = default!;

    public string CurrentTurn { get; set; } = default!;

    public string Result { get; set; } = default!;

    public string EndReason { get; set; } = default!;
}
