using KnightShift.Application.DTOs;

namespace KnightShift.Application.Interfaces;

public interface IGameService
{
    GameStateDto GetState();
    IEnumerable<MoveDto> GetLegalMoves();
    void ApplyMove(MoveDto moveDto);
    void StartNewGame();
    void LoadState(string serializedState);
    string ExportState();
    bool IsGameOver();
}
