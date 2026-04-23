using KnightShift.Application.Contracts.DTOs;

namespace KnightShift.Application.Contracts.Interfaces;

public interface IGameService
{
    GameStateDto GetState();
    IEnumerable<MoveDto> GetLegalMoves();
    IEnumerable<MoveDto> GetLegalMoves(string origin);
    IEnumerable<MoveDto> GetMoveHistory();
    IEnumerable<string> GetMoveHistoryFormatted();
    void ApplyMove(string serializedMove);
    void StartNewGame();
    void UndoMove();
    void RedoMove();
    void LoadState(string serializedState);
    string ExportState();
    bool IsGameOver();
}
