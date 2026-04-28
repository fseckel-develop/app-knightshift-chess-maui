using KnightShift.Application.Contracts.DTOs;

namespace KnightShift.Cli.Rendering.State;

public class UiState
{
    public GameStateDto Game { get; set; } = default!;
    public UiMode Mode { get; set; } = UiMode.Dashboard;
    public UiContent ContentType { get; set; } = UiContent.History;
    public object? ContentState { get; set; }
    public bool AutoPrintBoard { get; set; } = false;
    public bool PrintBoard { get; set; } = false;
    public string StatusMessage { get; set; } = " ";
}
