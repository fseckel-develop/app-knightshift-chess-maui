using KnightShift.Application.Contracts.DTOs;

namespace KnightShift.Application.Contracts.Interfaces;

public interface IGameService
{
    GameStateDto GetState();
    IEnumerable<MoveDto> GetLegalMoves();
    IEnumerable<MoveDto> GetLegalMoves(string origin);
    void ApplyMove(string serializedMove);
    void StartNewGame();
    void LoadState(string serializedState);
    string ExportState();
    bool IsGameOver();
}
