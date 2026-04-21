namespace KnightShift.Cli.Rendering;

public static class TextFrameRenderer
{
    public static string RenderFrame(string content)
    {
        using var renderer = new StringWriter();

        var lines = content.Split(Environment.NewLine);

        if (lines.Length > 0 && string.IsNullOrWhiteSpace(lines[^1]))
            lines = lines[..^1];

        int width = lines.Max(GetVisibleLength);

        renderer.Write(Ansi.Foreground(120, 120, 120));
        renderer.WriteLine($"┌{new string('─', width)}┐");

        foreach (var line in lines)
        {
            renderer.Write(Ansi.Foreground(120, 120, 120));
            renderer.Write("│");
            renderer.Write(Ansi.ResetColor());

            renderer.Write(line);
            int padding = width - GetVisibleLength(line);
            renderer.Write(new string(' ', padding));

            renderer.Write(Ansi.Foreground(120, 120, 120));
            renderer.WriteLine("│");
            renderer.Write(Ansi.ResetColor());
        }

        renderer.Write(Ansi.Foreground(120, 120, 120));
        renderer.WriteLine($"└{new string('─', width)}┘");
        renderer.Write(Ansi.ResetColor());

        return renderer.ToString();
    }

    private static int GetVisibleLength(string input)
    {
        int length = 0;
        bool inEscape = false;

        foreach (char symbol in input)
        {
            if (symbol == '\x1b') 
                inEscape = true;
            else if (inEscape && symbol == 'm') 
                inEscape = false;
            else if (!inEscape) 
                length++;
        }

        return length;
    }
}
