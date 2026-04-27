using System.Text;

namespace KnightShift.Cli.Rendering.Core;

public static class VerticalLayout
{
    public static string Combine(params string[] blocks)
    {
        var result = new StringBuilder();

        foreach (var block in blocks)
        {
            result.AppendLine(block);
        }

        return result.ToString();
    }
}
