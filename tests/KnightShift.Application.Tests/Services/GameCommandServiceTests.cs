using Microsoft.Extensions.DependencyInjection;
using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.DependencyInjection;
using KnightShift.Application.Services;
using KnightShift.Application.Game;
using KnightShift.Infrastructure.DependencyInjection;
using KnightShift.Engine.DependencyInjection;
using KnightShift.Domain.Enums;

namespace KnightShift.Application.Tests.Services;

public class GameCommandServiceTests
{
    private static (GameCommandService, GameSession) Create()
    {
        var services = new ServiceCollection()
            .AddApplication()
            .AddInfrastructure()
            .AddEngine()
            .BuildServiceProvider();

        var service = services.GetRequiredService<GameCommandService>();
        var factory = services.GetRequiredService<IGameStateFactory>();

        return (service, new GameSession(factory.CreateInitialState()));
    }

    [Fact]
    public void ApplyMove_ShouldApplyValidMove()
    {
        var (service, game) = Create();

        service.ApplyMove(game, "e2e4");

        Assert.Equal(PieceColor.Black, game.CurrentState.CurrentTurn);
    }

    [Fact]
    public void ApplyMove_ShouldThrow_OnIllegalMove()
    {
        var (service, game) = Create();

        Assert.Throws<InvalidOperationException>(() =>
            service.ApplyMove(game, "e2e5"));
    }

    [Fact]
    public void Undo_ShouldRevertMove()
    {
        var (service, game) = Create();

        service.ApplyMove(game, "e2e4");
        service.Undo(game);

        Assert.Equal(PieceColor.White, game.CurrentState.CurrentTurn);
    }

    [Fact]
    public void Undo_ShouldThrow_WhenNoMoves()
    {
        var (service, game) = Create();

        Assert.Throws<InvalidOperationException>(() => service.Undo(game));
    }

    [Fact]
    public void Redo_ShouldReapplyMove()
    {
        var (service, game) = Create();

        service.ApplyMove(game, "e2e4");
        service.Undo(game);
        service.Redo(game);

        Assert.Equal(PieceColor.Black, game.CurrentState.CurrentTurn);
    }

    [Fact]
    public void Redo_ShouldThrow_WhenNoRedoAvailable()
    {
        var (service, game) = Create();

        Assert.Throws<InvalidOperationException>(() => service.Redo(game));
    }
}
