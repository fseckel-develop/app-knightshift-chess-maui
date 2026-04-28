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
        }

        state.StatusMessage = result.Message ?? "";

        state.Mode = result.Mode ?? state.Mode;

        if (result.ContentType is not null)
        {
            state.ContentType = result.ContentType.Value;
            state.ContentState = result.ContentState;
        }
        else
        {
            state.ContentType = state.Mode switch
            {
                UiMode.Dashboard => UiContent.History,
                UiMode.Sequential => UiContent.None,
                _ => UiContent.None
            };

            state.ContentState = null;
        }
    }
}
