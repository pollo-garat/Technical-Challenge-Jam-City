using Pathfinding.Scripts.Gameplay.Domain.ValueObjects;
using UnityEngine;

namespace Pathfinding.Scripts.Gameplay.Domain.Views
{
    public class WorldGrid : MonoBehaviour
    {
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
        Material tileMaterial;

        public void Initialize()
        {
            AddGap();
            CalculateStartPosition();
        }

        public void Create(HexaTile[,] grid)
        {
            foreach (var tile in grid)
            {
                var hex = Instantiate(HexPrefab, transform);
                var gridPosition = new Vector2(tile.X, tile.Y);
                var unityHexaTile = hex.GetComponent<UnityHexaTile>();
                unityHexaTile.SaveConfiguration(tile.Configuration);
                hex.transform.position = CalculateWorldPosition(gridPosition);
                hex.name = "Hexagon " + tile.X + "|" + tile.Y + " Type " + tile.Configuration.Type;
                SetTileMaterial(hex, tile.Configuration.Type);
            }
        }

        void SetTileMaterial(GameObject hex, TileType type) =>
            hex.GetComponentInChildren<MeshRenderer>().sharedMaterial = type switch
            {
                TileType.Grass => GrassMaterial,
                TileType.Forest => ForestMaterial,
                TileType.Desert => DesertMaterial,
                TileType.Mountain => MountainMaterial,
                TileType.Water => WaterMaterial,
                _ => HexPrefab.GetComponent<MeshRenderer>().sharedMaterial
            };

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
