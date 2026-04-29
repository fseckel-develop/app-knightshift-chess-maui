using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.Contracts.DTOs;
using KnightShift.Cli.Execution;
using KnightShift.Cli.Rendering.State;
using NSubstitute;

namespace KnightShift.Cli.Tests.Rendering.State;

public class UiStateUpdaterTests
{
    private readonly IGameService _game = Substitute.For<IGameService>();
    private readonly UiStateUpdater _updater;

    public UiStateUpdaterTests()
    {
        _updater = new UiStateUpdater(_game);
    }

    private static UiState CreateState(UiMode mode = UiMode.Dashboard)
    {
        return new UiState
        {
            Mode = mode,
            Game = new GameStateDto(),
            ContentType = UiContent.History,
            AutoPrintBoard = false,
            PrintBoard = false
        };
    }

    [Fact]
    public void Apply_Should_Refresh_Game_State()
    {
        var newState = new GameStateDto();
        _game.GetState().Returns(newState);

        var state = CreateState();
        var result = new CommandResult { RefreshGameState = true };

        _updater.Apply(state, result);

        Assert.Equal(newState, state.Game);
    }

    [Fact]
    public void Apply_Should_Set_Message_When_No_Board_Flag()
    {
        var state = CreateState();
        var result = new CommandResult { Message = "hello" };

        _updater.Apply(state, result);

        Assert.Equal("hello", state.StatusMessage);
    }

    [Fact]
    public void Apply_Should_Override_Message_In_Dashboard_When_Board_Flag()
    {
        var state = CreateState(UiMode.Dashboard);

        var result = new CommandResult
        {
            Message = "should not show",
            PrintBoard = true
        };

        _updater.Apply(state, result);

        Assert.Equal("Only effective in sequential mode.", state.StatusMessage);
    }

    [Fact]
    public void Apply_Should_Not_Override_Message_In_Sequential_Mode()
    {
        var state = CreateState(UiMode.Sequential);

        var result = new CommandResult
        {
            Message = "ok",
            PrintBoard = true
        };

        _updater.Apply(state, result);

        Assert.Equal("ok", state.StatusMessage);
    }

    [Fact]
    public void Apply_Should_Update_Mode()
    {
        var state = CreateState();

        var result = new CommandResult
        {
            Mode = UiMode.Sequential
        };

        _updater.Apply(state, result);

        Assert.Equal(UiMode.Sequential, state.Mode);
    }

    [Fact]
    public void Apply_Should_Set_Content_When_Provided()
    {
        var state = CreateState();

        var result = new CommandResult
        {
            ContentType = UiContent.Moves,
            ContentState = new object()
        };

        _updater.Apply(state, result);

        Assert.Equal(UiContent.Moves, state.ContentType);
        Assert.NotNull(state.ContentState);
    }

    [Fact]
    public void Apply_Should_Default_To_History_In_Dashboard()
    {
        var state = CreateState(UiMode.Dashboard);

        var result = new CommandResult();

        _updater.Apply(state, result);

        Assert.Equal(UiContent.History, state.ContentType);
        Assert.Null(state.ContentState);
    }

    [Fact]
    public void Apply_Should_Default_To_None_In_Sequential()
    {
        var state = CreateState(UiMode.Sequential);

        var result = new CommandResult();

        _updater.Apply(state, result);

        Assert.Equal(UiContent.None, state.ContentType);
        Assert.Null(state.ContentState);
    }

    [Fact]
    public void Apply_Should_Update_AutoPrintBoard()
    {
        var state = CreateState();

        var result = new CommandResult
        {
            AutoPrintBoard = true
        };

        _updater.Apply(state, result);

        Assert.True(state.AutoPrintBoard);
    }

    [Fact]
    public void Apply_Should_PrintBoard_When_Explicit()
    {
        var state = CreateState();

        var result = new CommandResult
        {
            PrintBoard = true
        };

        _updater.Apply(state, result);

        Assert.True(state.PrintBoard);
    }

    [Fact]
    public void Apply_Should_AutoPrintBoard_On_Refresh()
    {
        var state = CreateState();
        state.AutoPrintBoard = true;

        var result = new CommandResult
        {
            RefreshGameState = true
        };

        _updater.Apply(state, result);

        Assert.True(state.PrintBoard);
    }
}
