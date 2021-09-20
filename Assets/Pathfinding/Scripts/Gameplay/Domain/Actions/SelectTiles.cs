using System;
using System.Collections.Generic;
using System.Linq;
using Pathfinding.Scripts.Gameplay.Domain.Repositories;
using Pathfinding.Scripts.Gameplay.Domain.ValueObjects;

namespace Pathfinding.Scripts.Gameplay.Domain.Actions
{
    public class SelectTiles
    {
        readonly SelectedTilesRepository selectedTilesRepository;

        public SelectTiles(SelectedTilesRepository selectedTilesRepository) => 
            this.selectedTilesRepository = selectedTilesRepository;

        public IEnumerable<HexaTile> Do(HexaTile tileSelected) => 
            tileSelected.Type() != TileType.Water && LoadSelectedTiles().All(tile => AreDistinct(tile, tileSelected)) ? 
                selectedTilesRepository.Save(LoadSelectedTiles().Concat(new[] {tileSelected})) : 
                LoadSelectedTiles();

        static bool AreDistinct(HexaTile tile, HexaTile tileSelected) => 
            tile.X != tileSelected.X || tile.Y != tileSelected.Y;

        IEnumerable<HexaTile> LoadSelectedTiles() => 
            selectedTilesRepository.Load();
    }
}