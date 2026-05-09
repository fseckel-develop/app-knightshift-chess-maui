using KnightShift.Application.Contracts.Interfaces;

namespace KnightShift.Application.UseCases.NewGame;

public class NewGameHandler : ICommandHandler<NewGameCommand>
{
    private readonly IGameService _game;

    public NewGameHandler(IGameService game)
    {
        _game = game;
    }

    public void Handle(NewGameCommand _)
    {
        _game.StartNewGame();
    }
}
