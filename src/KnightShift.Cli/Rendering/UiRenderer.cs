using KnightShift.Cli.Rendering.Content;
using KnightShift.Cli.Rendering.State;
using KnightShift.Cli.Rendering.Core;
using KnightShift.Cli.Rendering.Panels;

namespace KnightShift.Cli.Rendering;

public class UiRenderer
{
    private readonly IContentResolver _resolver;

    public UiRenderer(IContentResolver resolver)
    {
        _resolver = resolver;
    }

    public string Render(UiState state)
    {
        return state.Mode switch
        {
            UiMode.Dashboard => RenderDashboard(state),
            UiMode.Sequential => RenderSequential(state),
            _ => ""
        };
    }

    private string RenderDashboard(UiState state)
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

    private string RenderSequential(UiState state)
    {
        var output = new List<string>();

        if (!string.IsNullOrWhiteSpace(state.StatusMessage))
            output.Add("  " + state.StatusMessage);
        
        if (state.PrintBoard)
        {
            var board = BoardPanelRenderer.Render(state.Game);
            if (output.Count > 0)
                output.Add("");

            output.AddRange(board.Split("\n").Select(line => "  " + line));
        }

        var content = _resolver.Resolve(state);

        if (content.Length > 0 && content.Any(line => !string.IsNullOrWhiteSpace(line)))
        {
            if (!string.IsNullOrWhiteSpace(state.StatusMessage))
                output.Add("");
            
            output.AddRange(content);
        }

        output.Add("");

        return string.Join(Environment.NewLine, output);
    }
}
