using KnightShift.Application.UseCases.GetState;
using KnightShift.Cli.Execution;

namespace KnightShift.Cli.Rendering.State;

public class UiStateUpdater
{
    private readonly GetStateHandler _handler;

    public UiStateUpdater(GetStateHandler handler)
    {
        _handler = handler;
    }

    public void Apply(UiState state, CommandResult result)
    {
        if (result.RefreshGameState)
        {
            state.Game = _handler.Handle(new GetStateQuery());
        }

        if (state.Mode == UiMode.Dashboard && (result.AutoPrintBoard is not null || result.PrintBoard))
        {
            state.StatusMessage = "Only effective in sequential mode.";
        }
        else
        {
            state.StatusMessage = result.Message ?? "";
        }

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

        state.AutoPrintBoard = result.AutoPrintBoard ?? state.AutoPrintBoard;

        state.PrintBoard = result.PrintBoard || (state.AutoPrintBoard && result.RefreshGameState);

        state.ExitRequested = result.ExitRequested;
    }
}
