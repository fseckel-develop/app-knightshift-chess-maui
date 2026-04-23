using KnightShift.Application.Contracts.Interfaces;

namespace KnightShift.Cli.Execution.Commands;

public class FenCommand : ICommand
{
    private readonly IGameService _game;

    public string Name => "fen";

    public FenCommand(IGameService game)
    {
        _game = game;
    }

    public bool CanHandle(string input)
        => input.Equals(Name, StringComparison.OrdinalIgnoreCase);

    public Task ExecuteAsync(string input)
    {
        var fen = _game.ExportState();
        Console.WriteLine(fen);
        return Task.CompletedTask;
    }
}
