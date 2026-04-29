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
        Assert.Equal(PieceColorDto.White, state.CurrentTurn);
    }

    [Fact]
    public void StartNewGame_ShouldResetState()
    {
        var service = CreateService();

        service.ApplyMove("e2e4");

        service.StartNewGame();
        var state = service.GetState();

        Assert.Equal(PieceColorDto.White, state.CurrentTurn);
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

        service.ApplyMove("e2e4");

        var state = service.GetState();

        Assert.Equal(PieceColorDto.Black, state.CurrentTurn);
        Assert.Equal(PieceTypeDto.Pawn, state.Board[4, 4]!.Type);
    }

    [Fact]
    public void ApplyMove_ShouldThrow_OnIllegalMove()
    {
        var service = CreateService();

        Assert.Throws<InvalidOperationException>(() =>
            service.ApplyMove("e2e5")
        );
    }

    [Fact]
    public void UndoMove_ShouldRevertState()
    {
        var service = CreateService();

        service.ApplyMove("e2e4");
        service.UndoMove();

        var state = service.GetState();

        Assert.Equal(PieceColorDto.White, state.CurrentTurn);
    }

    [Fact]
    public void UndoMove_ShouldThrow_WhenNoHistory()
    {
        var service = CreateService();

        Assert.Throws<InvalidOperationException>(() => service.UndoMove());
    }

    [Fact]
    public void RedoMove_ShouldReapplyMove()
    {
        var service = CreateService();

        service.ApplyMove("e2e4");
        service.UndoMove();
        service.RedoMove();

        var state = service.GetState();

        Assert.Equal(PieceColorDto.Black, state.CurrentTurn);
    }

    [Fact]
    public void GetHistory_ShouldReturnMoveSteps()
    {
        var service = CreateService();

        service.ApplyMove("e2e4");
        service.ApplyMove("e7e5");

        var history = service.GetHistory().ToList();

        Assert.Equal(2, history.Count);
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
    public void ExportGame_ThenLoadGame_ShouldPreserveState()
    {
        var service = CreateService();

        service.ApplyMove("e2e4");
        service.ApplyMove("e7e5");

        var exported = service.ExportGame();

        var newService = CreateService();
        newService.LoadGame(exported);

        var state = newService.GetState();

        Assert.Equal(PieceColorDto.White, state.CurrentTurn);
    }

    [Fact]
    public void GetState_ShouldIndicateCheck()
    {
        var service = CreateService();

        service.LoadState("4k3/8/8/8/8/8/4R3/4K3 b - -");

        var state = service.GetState();

        Assert.True(state.CurrentIsInCheck);
    }

    [Fact]
    public void IsGameOver_ShouldBeFalse_AtStart()
    {
        var service = CreateService();

        var result = service.IsGameOver();

        Assert.False(result);
    }
}
