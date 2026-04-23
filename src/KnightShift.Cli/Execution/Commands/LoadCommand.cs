using KnightShift.Application.Contracts.Interfaces;

namespace KnightShift.Cli.Execution.Commands;

public class LoadCommand : ICommand
{
    private readonly IGameService _game;

    public string Name => "load";

    public LoadCommand(IGameService game)
    {
        _game = game;
    }

    public bool CanHandle(string input)
        => input.StartsWith(Name, StringComparison.OrdinalIgnoreCase);

    public Task ExecuteAsync(string input)
    {
        try
        {
            var fen = input[5..].Trim();
            _game.LoadState(fen);
            Console.WriteLine("Game loaded.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return Task.CompletedTask;
    }
}
