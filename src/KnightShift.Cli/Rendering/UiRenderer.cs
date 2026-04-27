using KnightShift.Cli.Rendering.Content;
using KnightShift.Cli.Rendering.State;
using KnightShift.Cli.Rendering.Core;
using KnightShift.Cli.Rendering.Panels;

namespace KnightShift.Cli.Rendering;

public class UiRenderer
{
    private readonly ContentResolver _resolver;

    public UiRenderer(ContentResolver resolver)
    {
        _resolver = resolver;
    }

    public string Render(UiState state)
    {
        var boardPanel = BoardPanelRenderer.Render(state.Game);

        var boardPanelLines = boardPanel.Split('\n');
        int panelWidth = boardPanelLines.Max(Ansi.GetVisibleLength);

        var messagePanel = MessagePanelRenderer.Render(state.StatusMessage, panelWidth);

        var leftPanel = VerticalLayout.Combine(
        [
            boardPanel,
            new string(' ', panelWidth),
            messagePanel
        ]);

        var leftPanelLines = leftPanel.Split('\n');
        int rightPanelHeight = leftPanelLines.Length;

        var rightPanelContent = _resolver.Resolve(state);

        var rightPanel = ContentPanelRenderer.Render(
            state.ContentType,
            rightPanelContent,
            rightPanelHeight
        );

        return HorizontalLayout.Combine(leftPanel, rightPanel);
    }
}
