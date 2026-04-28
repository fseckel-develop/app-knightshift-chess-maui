using KnightShift.Cli.Rendering.State;

namespace KnightShift.Cli.Execution.Commands;

public class UiModeCommand : ICommand
{
    public CommandInfo Info => new(
        Name: "ui",
        Aliases: ["mode"],
        Parameter: "{dash|seq}",
        Description: "Switch UI mode",
        Category: "System",
        Order: 0
    );

    public bool CanHandle(string input)
    {
        return input.StartsWith(Info.Name, StringComparison.OrdinalIgnoreCase) ||
            Info.Aliases.Any(alias => input.StartsWith(alias, StringComparison.OrdinalIgnoreCase));
    }
    
    public Task<CommandResult> ExecuteAsync(string input)
    {
        var commandParts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (commandParts.Length < 2)
        {
            return Task.FromResult(new CommandResult
            {
                Message = "No UI mode provided."
            });
        }

        var mode = commandParts[1].ToLower();

        return mode switch
        {
            "dashboard" or "dash" or "d" => Task.FromResult(new CommandResult
            {
                Mode = UiMode.Dashboard,
                Message = "Switched to dashboard mode."
            }),

            "sequential" or "seq" or "s" => Task.FromResult(new CommandResult
            {
                Mode = UiMode.Sequential,
                Message = "Switched to sequential mode."
            }),

            _ => Task.FromResult(new CommandResult
            {
                Message = "Unknown UI mode."
            })
        };
    }
}
