using System.Collections.Generic;
using System.Linq;
using Pathfinding.Scripts.Gameplay.Domain.Repositories;
using Pathfinding.Scripts.Gameplay.Domain.ValueObjects;

namespace Pathfinding.Scripts.Gameplay.Domain.Infrastructure
{
    public class InMemorySelectedTilesRepository : SelectedTilesRepository
    {
        IEnumerable<HexaTile> selectedTiles;

        public InMemorySelectedTilesRepository(IEnumerable<HexaTile> selectedTiles = null) => 
            this.selectedTiles = selectedTiles ?? Enumerable.Empty<HexaTile>();

        public IEnumerable<HexaTile> Load() => 
            selectedTiles;

        public IEnumerable<HexaTile> Save(IEnumerable<HexaTile> selectedTiles) =>
            this.selectedTiles = selectedTiles;

        public void Clear() => 
            selectedTiles = Enumerable.Empty<HexaTile>();
    }
}