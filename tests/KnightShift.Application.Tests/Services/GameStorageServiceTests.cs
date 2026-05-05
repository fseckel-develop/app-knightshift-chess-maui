using Microsoft.Extensions.DependencyInjection;
using KnightShift.Application.DependencyInjection;
using KnightShift.Application.Game.Services;
using KnightShift.Infrastructure.DependencyInjection;
using KnightShift.Engine.DependencyInjection;

namespace KnightShift.Application.Tests.Services;

public class GameStorageServiceTests
{
    private static GameStorageService Create()
    {
        var services = new ServiceCollection()
            .AddApplication()
            .AddInfrastructure()
            .AddEngine()
            .BuildServiceProvider();

        return services.GetRequiredService<GameStorageService>();
    }

    [Fact]
    public void LoadState_ShouldParseFen()
    {
        var service = Create();

        var game = service.LoadState("4k3/8/3p4/4P3/8/8/8/4K3 w - -");

        Assert.NotNull(game);
    }

    [Fact]
    public void ExportState_ShouldRoundtrip()
    {
        var service = Create();

        var game = service.LoadState("4k3/8/3p4/4P3/8/8/8/4K3 w - -");

        var fen = service.ExportState(game);

        Assert.Equal("4k3/8/3p4/4P3/8/8/8/4K3 w - -", fen);
    }

    [Fact]
    public void LoadGame_ShouldApplyMoves()
    {
        var service = Create();

        var game = service.LoadGame("1. e4 e5");

        Assert.NotNull(game);
    }

    [Fact]
    public void ExportGame_ShouldProduceString()
    {
        var service = Create();

        var game = service.LoadState("4k3/8/3p4/4P3/8/8/8/4K3 w - -");

        var result = service.ExportGame(game);

        Assert.NotNull(result);
    }
}
