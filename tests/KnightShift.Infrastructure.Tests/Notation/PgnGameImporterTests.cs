using KnightShift.Infrastructure.Notation;
using KnightShift.Infrastructure.Tests.Helpers;

namespace KnightShift.Infrastructure.Tests.Notation;

public class PgnGameImporterTests
{
    private static PgnGameImporter Importer() => 
        new(
            TestServices.Serializer(),
            TestServices.StateFactory(),
            TestServices.Resolver()
        );

    [Fact]
    public void Import_Should_Parse_Simple_Pgn()
    {
        var importer = Importer();

        var pgn = "1. e4 e5";

        var result = importer.Import(pgn);

        Assert.NotNull(result);
        Assert.NotEmpty(result.Moves);
    }

    [Fact]
    public void Import_Should_Use_Fen_If_Present()
    {
        var importer = Importer();

        var pgn = "[FEN \"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq -\"]\n1. e4";

        var result = importer.Import(pgn);

        Assert.NotNull(result.InitialState);
    }

    [Fact]
    public void Import_Should_Ignore_Comments_And_Variations()
    {
        var importer = Importer();

var pgn = @"
    [Event ""Test""]
    1. e4 e5 2. Nf3 Nc6
";

        var result = importer.Import(pgn);

        Assert.Equal(4, result.Moves.Count);
    }

    [Fact]
    public void Import_Should_Ignore_Result_Tokens()
    {
        var importer = Importer();

        var pgn = "1. e4 e5 1-0";

        var result = importer.Import(pgn);

        Assert.Equal(2, result.Moves.Count);
    }
}
