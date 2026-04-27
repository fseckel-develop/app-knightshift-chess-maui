using KnightShift.Cli.Rendering.Content;
using KnightShift.Cli.Rendering.State;
using KnightShift.Cli.Rendering.Core;
using KnightShift.Cli.Rendering.Panels;

namespace KnightShift.Cli.Rendering;

public class UiRenderer
{
    public static string Render(UiState state)
    {
        var boardPanel = BoardPanelRenderer.Render(state.Game);

        var boardPanelLines = boardPanel.Split('\n');
        int panelWidth = boardPanelLines.Max(Ansi.GetVisibleLength);

        var messagePanel = MessagePanelRenderer.Render(state.StatusMessage, panelWidth);

        var leftPanels = VerticalLayout.Combine(
        [
            boardPanel,
            new string(' ', panelWidth),
            messagePanel
        ]);

        return leftPanels;
    }
}
