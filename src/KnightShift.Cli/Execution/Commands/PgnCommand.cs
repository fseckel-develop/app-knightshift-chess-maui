using KnightShift.Application.Contracts.Interfaces;

namespace KnightShift.Cli.Execution.Commands;

public class PgnCommand : ICommand
{
    private readonly IGameService _game;

    public CommandInfo Info => new(
        Name: "pgn",
        Aliases: ["export-pgn", "save-pgn"],
        Parameter: "[file]",
        Description: "Show PGN or save to file",
        Category: "Import/Export",
        Order: 2
    );

    public PgnCommand(IGameService game)
    {
        _game = game;
    }

    public bool CanHandle(string input)
    {
        return input.StartsWith(Info.Name, StringComparison.OrdinalIgnoreCase) ||
            Info.Aliases.Any(alias => input.StartsWith(alias, StringComparison.OrdinalIgnoreCase));
    }

    public Task ExecuteAsync(string input)
    {
        var pgn = _game.ExportGame();

        var commandParts = input.Trim()
            .Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

        string? fileName = commandParts.Length > 1 ? commandParts[1] : null;

        if (!string.IsNullOrWhiteSpace(fileName))
        {
            if (fileName.Contains(' '))
                throw new InvalidOperationException("Invalid file name.");

            if (!fileName.EndsWith(".pgn"))
                fileName += ".pgn";

            File.WriteAllText(fileName, pgn);
            Console.WriteLine($"PGN saved to {fileName}");
        }
        else
        {
            Console.WriteLine(pgn);
        }

        return Task.CompletedTask;
    }
}
