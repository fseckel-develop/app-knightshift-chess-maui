using KnightShift.Application.Contracts.Interfaces;

namespace KnightShift.Cli.Execution.Commands;

public class LoadCommand : ICommand
{
    private readonly IGameService _game;

    public CommandInfo Info => new(
        Name: "load",
        Aliases: ["import", "open"],
        Parameter: "{fen|pgn|file}",
        Description: "Load game from input",
        Category: "Import/Export",
        Order: 0
    );

    public LoadCommand(IGameService game)
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
        var payload = CommandArgs.GetPayload(input, Info.Name);

        if (string.IsNullOrWhiteSpace(payload))
        {
            Console.WriteLine("Usage: load {fen|pgn|file}");
            return Task.CompletedTask;
        }

        string content;

        if (File.Exists(payload))
        {
            content = File.ReadAllText(payload);
        }
        else
        {
            content = payload;
        }

        try
        {
            if (LooksLikePgn(content))
            {
                _game.LoadGame(content);
                Console.WriteLine("PGN loaded.");
            }
            else if (LooksLikeFen(content))
            {
                _game.LoadState(content);
                Console.WriteLine("FEN loaded.");
            }
            else
            {
                Console.WriteLine("Unknown format.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Loading failed: {ex.Message}");
        }

        return Task.CompletedTask;
    }

    private static bool LooksLikePgn(string input)
        => input.Contains('[') || input.Contains("1.");

    private static bool LooksLikeFen(string input)
        => input.Contains('/') && input.Split(' ').Length >= 4;
}
