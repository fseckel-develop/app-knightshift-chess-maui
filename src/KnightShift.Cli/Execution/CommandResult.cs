using KnightShift.Cli.Rendering.State;

namespace KnightShift.Cli.Execution;

public class CommandResult
{
    public string Message { get; init; } = "";
    public UiContent? ContentType { get; init; }
    public string[]? PanelContent { get; init; }
    public bool RefreshGameState { get; init; } = false;
    public bool ExitRequested { get; init; } = false;
}
