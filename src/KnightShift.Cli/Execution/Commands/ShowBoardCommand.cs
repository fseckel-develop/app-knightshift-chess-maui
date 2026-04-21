using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Cli.Rendering;

namespace KnightShift.Cli.Execution.Commands;

public class ShowBoardCommand : ICommand
{
    private readonly IGameService _game;
    public string Name => "board";

    public ShowBoardCommand(IGameService game)
    {
        _game = game;
    }

    public bool CanHandle(string input)
        => input.Equals(Name, StringComparison.OrdinalIgnoreCase);

    public Task ExecuteAsync(string input)
    {
        var state = _game.GetState();
        var board = BoardRenderer.RenderBoard(state);
        var framed = TextFrameRenderer.RenderFrame(board);
        Console.Write(framed);
        
        return Task.CompletedTask;
    }
}
