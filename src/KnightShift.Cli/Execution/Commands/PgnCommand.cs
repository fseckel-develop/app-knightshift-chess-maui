using KnightShift.Application.UseCases.ExportGame;

namespace KnightShift.Cli.Execution.Commands;

public class PgnCommand : ICommand
{
    private readonly ExportGameHandler _handler;

    public CommandInfo Info => new(
        Name: "pgn",
        Aliases: ["save-game"],
        Parameter: "{file}",
        Description: "Export PGN to file",
        Category: "Import/Export",
        Order: 2
    );

    public PgnCommand(ExportGameHandler handler)
    {
        _handler = handler;
    }

    public bool CanHandle(string input)
    {
        return input.StartsWith(Info.Name, StringComparison.OrdinalIgnoreCase) ||
            Info.Aliases.Any(alias => input.StartsWith(alias, StringComparison.OrdinalIgnoreCase));
    }

    public Task<CommandResult> ExecuteAsync(string input)
    {
        var commandParts = input.Trim()
            .Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

        if (commandParts.Length < 2)
        {
            return Task.FromResult(new CommandResult
            {
                Message = "No file name provided."
            });
        }

        var fileName = commandParts[1];

        if (fileName.Contains(' '))
        {
            return Task.FromResult(new CommandResult
            {
                Message = "Invalid file name."
            });
        }

        if (!fileName.EndsWith(".pgn"))
            fileName += ".pgn";

        var pgn = _handler.Handle(new ExportGameQuery());
        File.WriteAllText(fileName, pgn);

        return Task.FromResult(new CommandResult
        {
            Message = $"PGN saved to {fileName}."
        });
    }
}
