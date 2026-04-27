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

        return Task.FromResult(new CommandResult
        {
            ContentType = UiContent.Help,
            Message = "Showing available commands."
        });
    }
}
