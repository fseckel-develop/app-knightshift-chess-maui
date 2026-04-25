using KnightShift.Application.Contracts.Interfaces;

namespace KnightShift.Cli.Execution.Commands;

public class PgnCommand : ICommand
{
    private readonly IGameService _game;

    public CommandInfo Info => new(
        Name: "pgn",
        Aliases: ["save-game"],
        Parameter: "{file}",
        Description: "Export PGN to file",
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
        var commandParts = input.Trim()
            .Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

        if (commandParts.Length < 2)
        {
            Console.WriteLine("No file name provided.");
            return Task.CompletedTask;
        }

        var fileName = commandParts[1];

        if (fileName.Contains(' '))
        {
            Console.WriteLine("Invalid file name.");
            return Task.CompletedTask;
        }

        if (!fileName.EndsWith(".pgn"))
            fileName += ".pgn";

        var pgn = _game.ExportGame();
        File.WriteAllText(fileName, pgn);

        Console.WriteLine($"PGN saved to {fileName}");
        return Task.CompletedTask;
    }
}
