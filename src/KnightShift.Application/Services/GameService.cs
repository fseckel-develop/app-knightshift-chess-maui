using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.Contracts.DTOs;
using KnightShift.Application.Game;

namespace KnightShift.Application.Services;

public class GameService : IGameService
{
    private readonly GameCommandService _commands;
    private readonly GameQueryService _queries;
    private readonly GameStorageService _storage;
    private readonly IGameStateFactory _factory;

    private GameSession _game;

    public GameService(
        GameCommandService commands,
        GameQueryService queries,
        GameStorageService storage,
        IGameStateFactory factory)
    {
        _commands = commands;
        _queries = queries;
        _storage = storage;
        _factory = factory;

        _game = new GameSession(factory.CreateInitialState());
    }

    public GameStateDto GetState() => _queries.GetState(_game);

    public IEnumerable<MoveDto> GetLegalMoves() => _queries.GetLegalMoves(_game);

    public IEnumerable<MoveDto> GetLegalMoves(string origin)
        => _queries.GetLegalMoves(_game, origin);

    public IEnumerable<MoveStep> GetHistory()
        => _queries.GetHistory(_game);

    public void ApplyMove(string move)
        => _commands.ApplyMove(_game, move);

    public void UndoMove()
        => _commands.Undo(_game);

    public void RedoMove()
        => _commands.Redo(_game);

    public void StartNewGame()
        => _game = _commands.StartNewGame(_factory);

    public void LoadState(string state)
        => _game = _storage.LoadState(state);

    public void LoadGame(string game)
        => _game = _storage.LoadGame(game);

    public string ExportState()
        => _storage.ExportState(_game);

    public string ExportGame()
        => _storage.ExportGame(_game);

    public bool IsGameOver()
        => _queries.IsGameOver(_game);
}
