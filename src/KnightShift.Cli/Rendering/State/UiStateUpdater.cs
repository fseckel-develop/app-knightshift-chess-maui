using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Cli.Execution;

namespace KnightShift.Cli.Rendering.State;

public class UiStateUpdater
{
    private readonly IGameService _game;

    public UiStateUpdater(IGameService game)
    {
        _game = game;
    }

    public void Apply(UiState state, CommandResult result)
    {
        if (result.RefreshGameState)
        {
            state.Game = _game.GetState();
            state.ContentType = UiContent.History;
        }

        state.StatusMessage = result.Message ?? "";

        state.ContentType = result.ContentType ?? UiContent.History;

        state.ContentState = result.ContentState;
    }
}
