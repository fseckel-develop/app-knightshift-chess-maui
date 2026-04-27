namespace KnightShift.Cli.Rendering.Core;

public static class Ansi
{
    public static string Background(int r, int g, int b) => $"\x1b[48;2;{r};{g};{b}m";

    public static string Foreground(int r, int g, int b) => $"\x1b[38;2;{r};{g};{b}m";

    public static string ResetColor() => "\x1b[0m";

    public static int GetVisibleLength(string input)
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

    public static string PadRightVisible(string input, int targetWidth)
    {
        int visible = GetVisibleLength(input);
        int padding = targetWidth - visible;

        if (padding <= 0)
            return input;

        return input + new string(' ', padding);
    }
}
