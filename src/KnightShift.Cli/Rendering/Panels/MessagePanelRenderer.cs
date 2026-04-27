using KnightShift.Cli.Rendering.Core;

namespace KnightShift.Cli.Rendering.Panels;

public static class MessagePanelRenderer
{
    public static string Render(string message, int panelWidth)
    {
        var lines = string.IsNullOrWhiteSpace(message)
            ? [" "]
            : (" " + message).Split(Environment.NewLine);

        var paddedLines = new List<string>();
        var messageWidth = panelWidth - 2; // substracting frame

        foreach (var line in lines)
        {
            var trimmed = line.Length > messageWidth
                ? line[..messageWidth]
                : line;

            var padding = Math.Max(0, messageWidth - trimmed.Length);
            paddedLines.Add(trimmed + new string(' ', padding));
        }

        var content = string.Join(Environment.NewLine, paddedLines);

        return FrameRenderer.RenderFrame(content);
    }
}
