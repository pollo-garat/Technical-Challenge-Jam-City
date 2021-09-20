using System;
using Pathfinding.Scripts.Gameplay.Domain.Services;
using Pathfinding.Scripts.Gameplay.Domain.ValueObjects;
using UniRx;
using UnityEngine;

namespace Pathfinding.Scripts.Gameplay.Domain.Views
{
    public class WorldGrid : MonoBehaviour
    {
        public IObservable<HexaTile> OnTileClicked => onTileClicked;
        
        readonly ISubject<HexaTile> onTileClicked = new Subject<HexaTile>();
        
        public GameObject HexPrefab;
        public int GridWidth;
        public int GridHeight;
        public float Gap;

        public Material GrassMaterial;
        public Material ForestMaterial;
        public Material DesertMaterial;
        public Material MountainMaterial;
        public Material WaterMaterial;
        
        float hexWidth = 1f;
        float hexHeight = 1f;
 
        Vector3 startPosition;
        HexaTile[,] grid;
        UnityHexaTile[,] unityGird;
        
        public void CreateGrid(HexaTile[,] grid, GridNeighbours girdNeighbours)
        {
            this.grid = grid;
            unityGird = new UnityHexaTile[GridWidth, GridHeight];
            AddGap();
            CalculateStartPosition();
            Create();
        }

        public UnityHexaTile[,] GetUnityGrid() => unityGird;

        void Create()
        {
            foreach (var tile in grid)
            {
                var hex = Instantiate(HexPrefab, transform);
                var gridPosition = new Vector2(tile.X, tile.Y);
                var unityHexaTile = hex.GetComponent<UnityHexaTile>();
                unityHexaTile.Populate(tile);
                hex.transform.position = CalculateWorldPosition(gridPosition);
                hex.name = "Hexagon " + tile.X + "|" + tile.Y + " Type " + tile.Configuration.Type;
                SetTileMaterial(hex, tile.Configuration.Type);
                unityGird[tile.X, tile.Y] = unityHexaTile;

                unityHexaTile.OnTileClicked.Subscribe(onTileClicked.OnNext);
            }
        }

        void SetTileMaterial(GameObject hex, TileType type)
        {
            var unityHexaTile = hex.GetComponent<UnityHexaTile>();
            
            switch (type)
            {
                case TileType.Grass:
                    unityHexaTile.SetInitialMaterial(GrassMaterial);
                    break;
                case TileType.Forest:
                    unityHexaTile.SetInitialMaterial(ForestMaterial);
                    break;
                case TileType.Desert:
                    unityHexaTile.SetInitialMaterial(DesertMaterial);
                    break;
                case TileType.Mountain:
                    unityHexaTile.SetInitialMaterial(MountainMaterial);
                    break;
                case TileType.Water:
                    unityHexaTile.SetInitialMaterial(WaterMaterial);
                    break;
                default:
                    unityHexaTile.SetInitialMaterial(GrassMaterial);
                    break;
            }
        }

        void AddGap()
        {
            hexWidth += hexWidth * Gap;
            hexHeight += hexHeight * Gap;
        }

        void CalculateStartPosition()
        {
            float offset = 0;
            if (GridHeight / 2 % 2 != 0)
                offset = hexWidth / 2;
 
            var x = -hexWidth * (GridWidth / 2) - offset;
            var z = hexHeight * 0.75f * (GridHeight / 2);
 
            startPosition = new Vector3(x, 0, z);
        }

        Vector3 CalculateWorldPosition(Vector2 gridPosition)
        {
            float offset = 0;
            if (gridPosition.y % 2 != 0)
                offset = hexWidth / 2;
 
            var x = startPosition.x + gridPosition.x * hexWidth + offset;
            var z = startPosition.z - gridPosition.y * hexHeight * 0.75f;
 
            return new Vector3(x, 0, z);
        }
    }
}
