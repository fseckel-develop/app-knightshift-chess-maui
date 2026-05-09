using KnightShift.Application.Contracts.Interfaces;
using KnightShift.Application.Contracts.DTOs;

namespace KnightShift.Application.UseCases.GetMoves;

public class GetMovesHandler
{
    private readonly IGameService _game;

    public GetMovesHandler(IGameService game)
    {
        _game = game;
    }

    public IEnumerable<MoveDto> Handle(GetMovesQuery query)
    {
        return query.Origin is null
            ? _game.GetLegalMoves()
            : _game.GetLegalMoves(query.Origin);
    }
}
