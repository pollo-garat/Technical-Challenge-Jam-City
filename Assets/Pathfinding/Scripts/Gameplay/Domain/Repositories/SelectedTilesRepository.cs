using System.Collections.Generic;
using Pathfinding.Scripts.Gameplay.Domain.ValueObjects;

namespace Pathfinding.Scripts.Gameplay.Domain.Repositories
{
    public interface SelectedTilesRepository
    {
        IEnumerable<HexaTile> Load();
        IEnumerable<HexaTile> Save(IEnumerable<HexaTile> selectedTiles);
    }
}