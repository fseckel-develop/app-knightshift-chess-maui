using KnightShift.Cli.Rendering.State;

namespace KnightShift.Cli.Execution.Commands;

public class HelpCommand : ICommand
{
    private IEnumerable<ICommand> _commands = [];

    public CommandInfo Info => new(
        Name: "help",
        Aliases: ["h", "?"],
        Parameter: null,
        Description: "Show this help",
        Category: "System",
        Order: 1
    );

    public void SetCommands(IEnumerable<ICommand> commands)
    {
        _commands = commands ?? throw new ArgumentNullException(nameof(commands));
    }

    public bool CanHandle(string input)
    {
        return input.Equals(Info.Name, StringComparison.OrdinalIgnoreCase) ||
            Info.Aliases.Any(alias => input.Equals(alias, StringComparison.OrdinalIgnoreCase));
    }

    public Task<CommandResult> ExecuteAsync(string input)
    {
        if (!_commands.Any())
        {
            return Task.FromResult(new CommandResult
            {
                Message = "No commands available."
            });
        }

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

        int commandWidth = entries.Max(entry => entry.Length) + 4;

        var content = new List<string>
        {
            "",
            $"   {"<uci>".PadRight(commandWidth)}Shortcut for move (e.g. e2e4)",
            ""
        };

        foreach (var group in grouped)
        {
            var ordered = group
                .OrderBy(command => command.Info.Order)
                .ThenBy(command => command.Info.Name);

            foreach (var command in ordered)
            {
                var label = BuildCommandLabel(command);
                content.Add($"   {label.PadRight(commandWidth)}{command.Info.Description}");
            }

            content.Add("");
        }

        return Task.FromResult(new CommandResult
        {
            ContentType = UiContent.Help,
            PanelContent = [.. content],
            Message = "Showing available commands."
        });
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
