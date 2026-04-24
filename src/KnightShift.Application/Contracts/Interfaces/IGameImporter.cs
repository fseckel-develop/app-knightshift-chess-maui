using KnightShift.Application.Game;

namespace KnightShift.Application.Contracts.Interfaces;

public interface IGameImporter
{
    GameRecord Import(string externalFormat);
}
