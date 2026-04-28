using KnightShift.Cli.Rendering.State;

namespace KnightShift.Cli.Execution;

public class CommandResult
{
    public string Message { get; init; } = "";
    public UiMode? Mode { get; init; }
    public UiContent? ContentType { get; init; }
    public object? ContentState { get; init; }
    public bool RefreshGameState { get; init; } = false;
    public bool ExitRequested { get; init; } = false;
}
