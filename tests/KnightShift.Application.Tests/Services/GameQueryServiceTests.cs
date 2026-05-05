using Microsoft.Extensions.DependencyInjection;
using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.DependencyInjection;
using KnightShift.Application.Services;
using KnightShift.Application.Game;
using KnightShift.Infrastructure.DependencyInjection;
using KnightShift.Infrastructure.Serialization;
using KnightShift.Engine.DependencyInjection;

namespace KnightShift.Application.Tests.Services;

public class GameQueryServiceTests
{
    private static readonly IGameStateSerializer _serializer = new FenGameStateSerializer();

    private static (GameQueryService, GameSession) Create()
    {
        var services = new ServiceCollection()
            .AddApplication()
            .AddInfrastructure()
            .AddEngine()
            .BuildServiceProvider();

        var service = services.GetRequiredService<GameQueryService>();
        var factory = services.GetRequiredService<IGameStateFactory>();

        return (service, new GameSession(factory.CreateInitialState()));
    }

    [Fact]
    public void GetLegalMoves_ShouldReturnMoves()
    {
        var (service, game) = Create();

        var moves = service.GetLegalMoves(game);

        Assert.NotEmpty(moves);
    }

    [Fact]
    public void GetLegalMoves_WithOrigin_ShouldFilterMoves()
    {
        var (service, game) = Create();

        var moves = service.GetLegalMoves(game, "e2").ToList();

        Assert.NotEmpty(moves);
        Assert.All(moves, move => Assert.Equal("e2", move.Origin));
    }

    [Fact]
    public void GetLegalMoves_WithInvalidOrigin_ShouldReturnEmpty()
    {
        var (service, game) = Create();

        var moves = service.GetLegalMoves(game, "e5"); // no piece initially

        Assert.Empty(moves);
    }

    [Fact]
    public void GetState_ShouldIndicateCheck()
    {
        var (service, _) = Create();

        var session = new GameSession(
            _serializer.Deserialize("4k3/8/8/8/8/8/4R3/4K3 b - -")
        );

        var dto = service.GetState(session);

        Assert.True(dto.CurrentIsInCheck);
    }

    [Fact]
    public void IsGameOver_ShouldBeFalse_AtStart()
    {
        var (service, game) = Create();

        var result = service.IsGameOver(game);

        Assert.False(result);
    }
}
