using System.Linq;
using Pathfinding.Scripts.Gameplay.Domain.Actions;
using Pathfinding.Scripts.Gameplay.Domain.Infrastructure;
using Pathfinding.Scripts.Gameplay.Domain.Services;
using Pathfinding.Scripts.Gameplay.Domain.ValueObjects;
using Pathfinding.Scripts.Gameplay.Domain.Views;
using UniRx;
using UnityEngine;

namespace Pathfinding.Scripts.Gameplay.Domain
{
    public class Context : MonoBehaviour
    {
        public WorldGrid WorldGrid;
        
        void Start()
        {
            var GrassConfiguration = new HexaTileConfiguration(TileType.Grass, 1, true);
            var ForestConfiguration = new HexaTileConfiguration(TileType.Forest, 3, true);
            var DesertConfiguration = new HexaTileConfiguration(TileType.Desert, 5, true);
            var MountainConfiguration = new HexaTileConfiguration(TileType.Mountain, 10, true);
            var WaterConfiguration = new HexaTileConfiguration(TileType.Water, 0, false);
            
            var randomTileService = new UnityRandomTileService(
                GrassConfiguration,
                ForestConfiguration,
                DesertConfiguration,
                MountainConfiguration,
                WaterConfiguration
            );
            
            var inMemorySelectedTilesRepository = new InMemorySelectedTilesRepository();
            
            var gridService = new GridService(randomTileService);
            var girdNeighbours = new GridNeighbours(WorldGrid.GridWidth, WorldGrid.GridHeight);
            
            var startGame = new StartGame(gridService);
            var selectTiles = new SelectTiles(inMemorySelectedTilesRepository);
            
            var domainGrid = startGame.Do(WorldGrid.GridWidth, WorldGrid.GridHeight);

            WorldGrid.CreateGrid(domainGrid, girdNeighbours);

            // WorldGrid.OnTileClicked
            //     .Select(hexaTile => girdNeighbours.ValidNeighbours(hexaTile))
            //     .Do(neighbours => Debug.Log(string.Join(" ", neighbours.Select(x => x.ToString()))))
            //     .Do(neighbours =>
            //     {
            //         if(WorldGrid.DebugMode)
            //             WorldGrid.PaintNeighbours(neighbours);
            //     })
            //     .Subscribe();
            
            WorldGrid.OnTileClicked
                .Select(hexaTile => selectTiles.Do(hexaTile))
                .Do(selectedTiles => WorldGrid.FindPath(selectedTiles))
                .Subscribe();
        }
    }
}
