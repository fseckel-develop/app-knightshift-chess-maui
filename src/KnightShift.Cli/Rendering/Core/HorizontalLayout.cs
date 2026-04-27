namespace KnightShift.Cli.Rendering.Core;

public static class HorizontalLayout
{
    public static string Combine(string leftBlock, string rightBlock, int spacing = 3)
    {
        if (string.IsNullOrWhiteSpace(rightBlock))
            return leftBlock;

        var leftLines = leftBlock.Split('\n');
        var rightLines = rightBlock.Split('\n');

        int height = Math.Max(leftLines.Length, rightLines.Length);

        var result = new List<string>();

        for (int i = 0; i < height; i++)
        {
            var left = i < leftLines.Length ? leftLines[i] : "";
            var right = i < rightLines.Length ? rightLines[i] : "";

            result.Add(left + new string(' ', spacing) + right);
        }

        return string.Join(Environment.NewLine, result);
    }
}
