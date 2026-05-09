using KnightShift.Application.Contracts.Interfaces;

namespace KnightShift.Application.UseCases.LoadState;

public class LoadStateHandler : ICommandHandler<LoadStateCommand>
{
    private readonly IGameService _game;

    public LoadStateHandler(IGameService game)
    {
        _game = game;
    }

    public void Handle(LoadStateCommand command)
    {
        _game.LoadState(command.Fen);
    }
}
