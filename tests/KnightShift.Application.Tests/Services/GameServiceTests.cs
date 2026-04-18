using Microsoft.Extensions.DependencyInjection;
using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.Contracts.DTOs;
using KnightShift.Application.DependencyInjection;
using KnightShift.Infrastructure.DependencyInjection;
using KnightShift.Engine.DependencyInjection;

namespace KnightShift.Application.Tests.Services;

public class GameServiceTests
{
    private static IGameService CreateService()
    {
        var services = new ServiceCollection();

        services
            .AddApplication()
            .AddInfrastructure()
            .AddEngine();

        var provider = services.BuildServiceProvider();

        return provider.GetRequiredService<IGameService>();
    }

    [Fact]
    public void StartNewGame_ShouldInitializeBoard()
    {
        var service = CreateService();

        service.StartNewGame();
        var state = service.GetState();

        Assert.NotNull(state);
        Assert.Equal("White", state.CurrentTurn);
    }

    [Fact]
    public void StartNewGame_ShouldResetState()
    {
        var service = CreateService();

        service.ApplyMove(new MoveDto { Origin = "e2", Target = "e4" });

        service.StartNewGame();
        var state = service.GetState();

        Assert.Equal("White", state.CurrentTurn);
    }

    [Fact]
    public void GetLegalMoves_ShouldReturnMoves()
    {
        var service = CreateService();

        var moves = service.GetLegalMoves();

        Assert.NotEmpty(moves);
    }

    [Fact]
    public void GetLegalMoves_WithOrigin_ShouldFilterMoves()
    {
        var service = CreateService();

        var moves = service.GetLegalMoves("e2").ToList();

        Assert.All(moves, move => Assert.Equal("e2", move.Origin));
        Assert.NotEmpty(moves);
    }

    [Fact]
    public void ApplyMove_ShouldMovePawn_AndSwitchTurn()
    {
        var service = CreateService();

        service.ApplyMove(new MoveDto { Origin = "e2", Target = "e4" });

        var state = service.GetState();

        Assert.Equal("Black", state.CurrentTurn);
        Assert.Equal("P", state.Board[4][4]);
    }

    [Fact]
    public void LoadState_ThenExportState_ShouldBeConsistent()
    {
        var service = CreateService();

        var fen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq -";

        service.LoadState(fen);
        var result = service.ExportState();

        Assert.Equal(fen, result);
    }

    [Fact]
    public void IsGameOver_ShouldBeFalse_AtStart()
    {
        var service = CreateService();

        var result = service.IsGameOver();

        Assert.False(result);
    }
}
