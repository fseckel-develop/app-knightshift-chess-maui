using KnightShift.Application.Contracts.Interfaces;

namespace KnightShift.Cli.Execution.Commands;

public class UndoCommand : ICommand
{
    private readonly IGameService _game;

    public string Name => "undo";

    public UndoCommand(IGameService game)
    {
        _game = game;
    }

    public bool CanHandle(string input)
        => input.Equals(Name, StringComparison.OrdinalIgnoreCase);

    public Task ExecuteAsync(string input)
    {
        try
        {
            var state = _game.GetState();
            _game.UndoMove();
            Console.WriteLine($"Move {state.LastMove!.Origin}{state.LastMove!.Target} undone.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return Task.CompletedTask;
    }
}
