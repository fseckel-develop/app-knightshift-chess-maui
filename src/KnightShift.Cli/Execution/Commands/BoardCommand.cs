namespace KnightShift.Cli.Execution.Commands;

public class BoardCommand : ICommand
{
    public CommandInfo Info => new(
        Name: "board",
        Aliases: ["display"],
        Parameter: "[on|off]",
        Description: "Show board and set auto-print",
        Category: "View",
        Order: 1
    );

    public bool CanHandle(string input)
    {
        return input.StartsWith(Info.Name, StringComparison.OrdinalIgnoreCase) ||
            Info.Aliases.Any(alias => input.StartsWith(alias, StringComparison.OrdinalIgnoreCase));
    }

    public Task<CommandResult> ExecuteAsync(string input)
    {
        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 2)
        {
            return parts[1].ToLower() switch
            {
                "on" => Task.FromResult(new CommandResult
                {
                    AutoPrintBoard = true,
                    Message = "Auto-printing of board enabled."
                }),

                "off" => Task.FromResult(new CommandResult
                {
                    AutoPrintBoard = false,
                    Message = "Auto-printing of board disabled."
                }),

                _ => Task.FromResult(new CommandResult
                {
                    Message = "Unkown auto-printing state."
                })
            };
        }

        return Task.FromResult(new CommandResult
        {
            Message = "Showing current board state.",
            PrintBoard = true
        });
    }
}
