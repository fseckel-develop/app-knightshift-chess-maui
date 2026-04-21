namespace KnightShift.Cli.Rendering;

public static class Ansi
{
    public static string Background(int r, int g, int b) => $"\x1b[48;2;{r};{g};{b}m";

    public static string Foreground(int r, int g, int b) => $"\x1b[38;2;{r};{g};{b}m";

    public static string ResetColor() => "\x1b[0m";
}
