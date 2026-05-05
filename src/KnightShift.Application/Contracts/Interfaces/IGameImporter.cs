using KnightShift.Application.Game.Models;

namespace KnightShift.Application.Contracts.Interfaces;

public interface IGameImporter
{
    GameRecord Import(string externalFormat);
}
