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
            var gridService = new GridService(randomTileService);
            var startGame = new StartGame(gridService);

            WorldGrid.Initialize();

            var domainGrid = startGame.Do(WorldGrid.GridWidth, WorldGrid.GridHeight);

            WorldGrid.Create(domainGrid);

            WorldGrid.OnTileClicked
                .Subscribe();
        }
    }
}
