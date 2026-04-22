using KnightShift.Application.Contracts.Interfaces;

namespace KnightShift.Cli.Execution.Commands;

public class NewGameCommand : ICommand
{
    private readonly IGameService _game;

    public string Name => "new";

    public NewGameCommand(IGameService game)
    {
        _game = game;
    }

    public bool CanHandle(string input)
        => input.Equals(Name, StringComparison.OrdinalIgnoreCase);

    public Task ExecuteAsync(string input)
    {
        _game.StartNewGame();
        Console.WriteLine("New game started.");
        return Task.CompletedTask;
    }
}
