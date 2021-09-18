using System.Collections.Generic;
using System.Linq;
using Pathfinding.Scripts.Gameplay.Domain.Repositories;

namespace Pathfinding.Scripts.Gameplay.Domain.Actions
{
    public class SelectTiles
    {
        readonly SelectedTilesRepository selectedTilesRepository;

        public SelectTiles(SelectedTilesRepository selectedTilesRepository) => 
            this.selectedTilesRepository = selectedTilesRepository;

        public IEnumerable<(int, int)> Do((int, int) tileSelected) => 
            selectedTilesRepository.Save(
                LoadSelectedTiles().Concat(new[] {tileSelected})
            );

        IEnumerable<(int, int)> LoadSelectedTiles() => 
            selectedTilesRepository.Load();
    }
}