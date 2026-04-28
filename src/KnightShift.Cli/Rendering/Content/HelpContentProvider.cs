using KnightShift.Cli.Execution.Commands;
using KnightShift.Cli.Rendering.State;

namespace KnightShift.Cli.Rendering.Content;

public class HelpContentProvider : IContentProvider
{
    private readonly IEnumerable<ICommand> _commands;

    public UiContent ContentType => UiContent.Help;

    public HelpContentProvider(IEnumerable<ICommand> commands)
    {
        _commands = commands;
    }

    public string[] GetContent(UiState state)
    {
        var categoryOrder = new[] { "Game", "View", "Import/Export", "System" };

        var grouped = _commands
            .GroupBy(command => command.Info.Category)
            .OrderBy(group =>
            {
                var index = Array.IndexOf(categoryOrder, group.Key);
                return index >= 0 ? index : int.MaxValue;
            });

        var entries = grouped
            .SelectMany(group => group)
            .Select(BuildCommandLabel)
            .ToList();

        int commandWidth = entries.Count != 0
            ? entries.Max(entry => entry.Length) + 3
            : 10;

        var content = new List<string>();

        if (state.Mode == UiMode.Dashboard)
            content.Add("");
            
        content.Add($"  {"<uci>".PadRight(commandWidth)}Shortcut for move (e.g. e2e4)");

        foreach (var group in grouped)
        {
            if (state.Mode == UiMode.Dashboard)
                content.Add("");

            var ordered = group
                .OrderBy(command => command.Info.Order)
                .ThenBy(command => command.Info.Name);

            foreach (var command in ordered)
            {
                var label = BuildCommandLabel(command);
                content.Add($"  {label.PadRight(commandWidth)}{command.Info.Description}");
            }
        }

        return [.. content];
    }

    private static string BuildCommandLabel(ICommand command)
    {
        var name = command.Info.Name;

        var aliases = command.Info.Aliases.Any()
            ? $"({string.Join(", ", command.Info.Aliases)})"
            : "";

        var parameter = string.IsNullOrWhiteSpace(command.Info.Parameter)
            ? ""
            : command.Info.Parameter;

        return $"{name} {aliases} {parameter}".Trim();
    }
}
