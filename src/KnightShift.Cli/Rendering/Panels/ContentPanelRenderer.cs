using KnightShift.Cli.Rendering.State;
using KnightShift.Cli.Rendering.Core;

namespace KnightShift.Cli.Rendering.Panels;

public static class ContentPanelRenderer
{
    public static string Render(UiContent type, string[] content, int panelHeight = 0)
    {
        int contentWidth = GetContentWidth(type);
        int contentHeight = panelHeight - 2; // substracting frame

        var lines = new List<string>
        {
            $" {GetPanelTitle(type)}",
            new('─', contentWidth)
        };

        foreach (var line in content ?? [])
        {
            var trimmed = line.Length > contentWidth ? line[..contentWidth] : line;
            var padded = trimmed.PadRight(contentWidth);
            lines.Add(padded);
        }
        
        while (lines.Count < contentHeight)
            lines.Add(new string(' ', contentWidth));

        if (lines.Count > contentHeight)
            lines = [.. lines.Take(contentHeight)];

        return FrameRenderer.RenderFrame(string.Join(Environment.NewLine, lines));
    }

    private static int GetContentWidth(UiContent type) => type switch
    {
        UiContent.Moves   => 24,
        UiContent.History => 24,
        UiContent.Help    => 65,
        _                 => 24
    };

    private static string GetPanelTitle(UiContent type) => type switch
    {
        UiContent.History => "Move History",
        UiContent.Moves   => "Legal Moves",
        UiContent.Help    => "Available Commands",
        _                 => type.ToString()
    };
}
