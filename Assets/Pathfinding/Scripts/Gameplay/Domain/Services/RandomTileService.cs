using Pathfinding.Scripts.Gameplay.Domain.ValueObjects;

namespace Pathfinding.Scripts.Gameplay.Domain.Services
{
    public interface RandomTileService
    {
        HexaTile PickOne();
    }
}