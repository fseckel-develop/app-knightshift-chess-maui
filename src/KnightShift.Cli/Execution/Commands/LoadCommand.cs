using KnightShift.Application.Contracts.Interfaces;

namespace KnightShift.Cli.Execution.Commands;

public class LoadCommand : ICommand
{
    private readonly IGameService _game;

    public CommandInfo Info => new(
        Name: "load",
        Aliases: ["open"],
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

    public Task<CommandResult> ExecuteAsync(string input)
    {
        var commandParts = input.Trim()
            .Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

        if (commandParts.Length < 2)
        {
            return Task.FromResult(new CommandResult
            {
                Message = "No payload or file name provided."
            });
        }

        var payload = commandParts[1];

        string content = File.Exists(payload)
            ? File.ReadAllText(payload)
            : payload;

        try
        {
            if (LooksLikePgn(content))
            {
                _game.LoadGame(content);

                return Task.FromResult(new CommandResult
                {
                    Message = "PGN loaded.",
                    RefreshGameState = true
                });
            }

            if (LooksLikeFen(content))
            {
                _game.LoadState(content);

                return Task.FromResult(new CommandResult
                {
                    Message = "FEN loaded.",
                    RefreshGameState = true
                });
            }

            return Task.FromResult(new CommandResult
            {
                Message = "Unknown format."
            });
        }
        catch (Exception ex)
        {
            return Task.FromResult(new CommandResult
            {
                Message = ex.Message
            });
        }
    }

    private static bool LooksLikePgn(string input)
        => input.Contains('[') || input.Contains("1.");

    private static bool LooksLikeFen(string input)
        => input.Contains('/') && input.Split(' ').Length >= 4;
}
