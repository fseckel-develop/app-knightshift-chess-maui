using KnightShift.Application.Contracts.DTOs;

namespace KnightShift.Cli.Rendering.State;

public class UiState
{
    public GameStateDto Game { get; set; } = default!;
    public UiMode Mode { get; set; } = UiMode.Dashboard;
    public UiContent ContentType { get; set; } = UiContent.History;
    public string[] PanelContent { get; set; } = [];
    public string StatusMessage { get; set; } = " ";
}
