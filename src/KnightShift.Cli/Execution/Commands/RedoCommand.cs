using KnightShift.Application.Contracts.Interfaces;

namespace KnightShift.Cli.Execution.Commands;

public class RedoCommand : ICommand
{
    private readonly IGameService _game;

    public string Name => "redo";

    public RedoCommand(IGameService game)
    {
        _game = game;
    }

    public bool CanHandle(string input)
        => input.Equals(Name, StringComparison.OrdinalIgnoreCase);

    public Task ExecuteAsync(string input)
    {
        try
        {
            _game.RedoMove();
            var state = _game.GetState();
            Console.WriteLine($"Move {state.LastMove!.Origin}{state.LastMove!.Target} redone.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return Task.CompletedTask;
    }
}
