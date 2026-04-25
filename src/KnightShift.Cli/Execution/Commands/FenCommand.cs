using KnightShift.Application.Contracts.Interfaces;

namespace KnightShift.Cli.Execution.Commands;

public class FenCommand : ICommand
{
    private readonly IGameService _game;

    public CommandInfo Info => new(
        Name: "fen",
        Aliases: ["export-fen", "save-fen"],
        Parameter: "[file]",
        Description: "Show FEN or save to file",
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

    public Task ExecuteAsync(string input)
    {
        var fen = _game.ExportState();
        Console.WriteLine(fen);
        return Task.CompletedTask;
    }
}
