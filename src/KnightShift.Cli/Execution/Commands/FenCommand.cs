using KnightShift.Application.Contracts.Interfaces;

namespace KnightShift.Cli.Execution.Commands;

public class FenCommand : ICommand
{
    private readonly IGameService _game;

    public CommandInfo Info => new(
        Name: "fen",
        Aliases: ["save-state"],
        Parameter: "{file}",
        Description: "Export FEN to file",
        Category: "Import/Export",
        Order: 1
    );

    public FenCommand(IGameService game)
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

        if (!fileName.EndsWith(".fen"))
            fileName += ".fen";

        var fen = _game.ExportState();
        File.WriteAllText(fileName, fen);

        return Task.FromResult(new CommandResult
        {
            Message = $"FEN saved to {fileName}."
        });
    }
}
