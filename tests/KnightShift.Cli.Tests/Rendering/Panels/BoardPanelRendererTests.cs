using KnightShift.Cli.Rendering.Panels;
using KnightShift.Application.Contracts.DTOs;

namespace KnightShift.Cli.Tests.Rendering.Panels;

public class BoardPanelRendererTests
{
    [Fact]
    public void Render_Should_Return_Frame()
    {
        var state = CreateEmptyState();

        var result = BoardPanelRenderer.Render(state);

        Assert.Contains("┌", result);
        Assert.Contains("└", result);
    }

    [Fact]
    public void Render_Should_Contain_Files()
    {
        var state = CreateEmptyState();

        var result = BoardPanelRenderer.Render(state);

        Assert.Contains("a", result);
        Assert.Contains("h", result);
    }

    private static GameStateDto CreateEmptyState()
    {
        return new GameStateDto
        {
            Board = new PieceDto?[8,8],
            LastMove = null
        };
    }
}
