using KnightShift.Infrastructure.Notation;
using KnightShift.Domain.Enums;
using KnightShift.Infrastructure.Tests.Helpers;

namespace KnightShift.Infrastructure.Tests.Notation;

public class SanMoveResolverTests
{
    [Fact]
    public void Resolve_Should_Parse_Simple_Move()
    {
        var resolver = TestServices.Resolver();

        var state = TestGameStateFactory.CreateFromFen("4k3/8/8/8/8/8/4P3/4K3 w - -");

        var move = resolver.Resolve("e4", state);

        Assert.Equal("e2", move.Origin.ToString());
        Assert.Equal("e4", move.Target.ToString());
    }

    [Fact]
    public void Resolve_Should_Handle_Castling()
    {
        var resolver = TestServices.Resolver();

        var state = TestGameStateFactory.CreateFromFen("4k3/8/8/8/8/8/8/4K2R w K -");

        var move = resolver.Resolve("O-O", state);

        Assert.True(move.IsCastling);
    }

    [Fact]
    public void Resolve_Should_Throw_On_Invalid_Move()
    {
        var resolver = TestServices.Resolver();

        var state = TestGameStateFactory.CreateFromFen("4k3/8/8/8/8/8/8/4K3 w - -");

        Assert.Throws<InvalidOperationException>(() => resolver.Resolve("e4", state));
    }

    [Fact]
    public void Resolve_Should_Parse_Pawn_Capture()
    {
        var resolver = TestServices.Resolver();

        var state = TestGameStateFactory.CreateFromFen("4k3/8/3p4/4P3/8/8/8/4K3 w - -");

        var move = resolver.Resolve("exd6", state);

        Assert.Equal("e5", move.Origin.ToString());
        Assert.Equal("d6", move.Target.ToString());
    }

    [Fact]
    public void Resolve_Should_Parse_Promotion()
    {
        var resolver = TestServices.Resolver();

        var state = TestGameStateFactory.CreateFromFen("6k1/4P3/8/8/8/8/8/4K3 w - -");

        var move = resolver.Resolve("e8=Q", state);

        Assert.Equal(PieceType.Queen, move.Promotion);
    }

    [Fact]
    public void Resolve_Should_Throw_On_Ambiguous_Move()
    {
        var resolver = TestServices.Resolver();

        var state = TestGameStateFactory.CreateFromFen("4k3/8/8/8/3N1N2/8/8/4K3 w - -");

        Assert.Throws<InvalidOperationException>(() =>
            resolver.Resolve("Ne6", state)
        );
    }

    [Fact]
    public void Resolve_Should_Handle_Disambiguation()
    {
        var resolver = TestServices.Resolver();

        var state = TestGameStateFactory.CreateFromFen("4k3/8/8/8/3N1N2/8/8/4K3 w - -");

        var move = resolver.Resolve("Ndf5", state);

        Assert.Equal("d4", move.Origin.ToString());
    }

    [Fact]
    public void Resolve_Should_Handle_EnPassant()
    {
        var resolver = TestServices.Resolver();

        var state = TestGameStateFactory.CreateFromFen("4k3/8/8/3pP3/8/8/8/4K3 w - d6");

        var move = resolver.Resolve("exd6", state);

        Assert.True(move.IsEnPassant);
    }
}
