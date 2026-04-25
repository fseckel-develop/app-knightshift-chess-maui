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

    public Task ExecuteAsync(string input)
    {
        if (!_commands.Any())
        {
            Console.WriteLine("No commands available.");
            return Task.CompletedTask;
        }
        
        int namesWidth = _commands.Max(command => command.Info.Name.Length) + 2;
        int parametersWidth = _commands.Max(command => (command.Info.Parameter ?? "").Length) + 2;
        int aliasesWidth = _commands.Max(command => 
            command.Info.Aliases.Any() ? $"({string.Join(", ", command.Info.Aliases)})".Length : 0) + 2;

        Console.WriteLine("Available commands:\n");

        var order = new[] { "Game", "View", "Import/Export", "System" };

        var grouped = _commands
            .GroupBy(command => command.Info.Category)
            .OrderBy(group =>
            {
                var index = Array.IndexOf(order, group.Key);
                return index >= 0 ? index : int.MaxValue;
            });

        Console.WriteLine($"  {"<uci>".PadRight(namesWidth)}{"".PadRight(parametersWidth + aliasesWidth)}{"Directly play move (e.g. e2e4)"}");

        foreach (var group in grouped)
        {
            var entries = group
                .Select(command => new
                {
                    command.Info.Name,
                    command.Info.Order,
                    Parameters = string.IsNullOrWhiteSpace(command.Info.Parameter) ? "" : command.Info.Parameter,
                    Aliases = command.Info.Aliases.Any() ? $"({string.Join(", ", command.Info.Aliases)})" : "",
                    command.Info.Description
                })
                .OrderBy(command => command.Order)
                .ThenBy(command => command.Name)
                .ToList();

            foreach (var entry in entries)
            {
                Console.WriteLine(
                    $"  {entry.Name.PadRight(namesWidth)}" +
                    $"{entry.Parameters.PadRight(parametersWidth)}" +
                    $"{entry.Aliases.PadRight(aliasesWidth)}" +
                    $"{entry.Description}"
                );
            }

            Console.WriteLine();
        }

        return Task.CompletedTask;
    }
}
