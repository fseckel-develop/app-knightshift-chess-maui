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

        return services.BuildServiceProvider()
            .GetRequiredService<IGameService>();
    }

    [Fact]
    public void StartNewGame_ShouldInitializeBoard()
    {
        var service = CreateService();

        service.StartNewGame();
        var state = service.GetState();

        Assert.Equal(PieceColorDto.White, state.CurrentTurn);
    }

    [Fact]
    public void ApplyMove_ShouldDelegateCorrectly()
    {
        var service = CreateService();

        service.ApplyMove("e2e4");

        var state = service.GetState();

        Assert.Equal(PieceColorDto.Black, state.CurrentTurn);
    }

    [Fact]
    public void UndoMove_ShouldDelegateCorrectly()
    {
        var service = CreateService();

        service.ApplyMove("e2e4");
        service.UndoMove();

        var state = service.GetState();

        Assert.Equal(PieceColorDto.White, state.CurrentTurn);
    }

    [Fact]
    public void ExportGame_ThenLoadGame_ShouldWorkThroughFacade()
    {
        var service = CreateService();

        service.ApplyMove("e2e4");
        var exported = service.ExportGame();

        var newService = CreateService();
        newService.LoadGame(exported);

        var state = newService.GetState();

        Assert.Equal(PieceColorDto.Black, state.CurrentTurn);
    }
}
