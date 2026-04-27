namespace KnightShift.Cli.Rendering.Core;

public static class FrameRenderer
{
    public static string RenderFrame(string content)
    {
        using var renderer = new StringWriter();

        var lines = content.Split(Environment.NewLine);

        if (lines.Length > 1 && string.IsNullOrWhiteSpace(lines[^1]))
            lines = lines[..^1];

        if (lines.Length == 0)
            return "";

        int width = lines.Max(Ansi.GetVisibleLength);

        renderer.Write(Ansi.Foreground(120, 120, 120));
        renderer.WriteLine($"┌{new string('─', width)}┐");
        renderer.Write(Ansi.ResetColor());

        foreach (var line in lines)
        {
            renderer.Write(Ansi.Foreground(120, 120, 120));
            renderer.Write("│");
            renderer.Write(Ansi.ResetColor());

            renderer.Write(line);
            int padding = width - Ansi.GetVisibleLength(line);
            renderer.Write(new string(' ', padding));

            renderer.Write(Ansi.Foreground(120, 120, 120));
            renderer.Write("│");
            renderer.WriteLine(Ansi.ResetColor());
        }

        renderer.Write(Ansi.Foreground(120, 120, 120));
        renderer.Write($"└{new string('─', width)}┘");
        renderer.Write(Ansi.ResetColor());

        return renderer.ToString();
    }
}
