using KnightShift.Application.Contracts.DTOs;
using KnightShift.Application.Game;

namespace KnightShift.Application.Contracts.Interfaces;

public interface IGameService
{
    GameStateDto GetState();
    IEnumerable<MoveDto> GetLegalMoves();
    IEnumerable<MoveDto> GetLegalMoves(string origin);
    IEnumerable<MoveStep> GetHistory();
    void ApplyMove(string serializedMove);
    void UndoMove();
    void RedoMove();
    void StartNewGame();
    void LoadState(string serializedState);
    string ExportState();
    bool IsGameOver();
}
