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
 
        Vector3 startPos;

        public void Initialize()
        {
            AddGap();
            CalculateStartPosition();
        }

        public void Create(HexaTile[,] grid)
        {
            foreach (var tile in grid)
            {
                var hex = Instantiate(HexPrefab, transform, true);
                var gridPos = new Vector2(tile.X, tile.Y);
                hex.transform.position = CalculateWorldPosition(gridPos);
                hex.name = "Hexagon" + tile.X + "|" + tile.Y;
                SetTileMaterial(tile.Configuration.Type);
            }
        }

        void SetTileMaterial(TileType type) =>
            HexPrefab.GetComponentInChildren<MeshRenderer>().material = type switch
            {
                TileType.Grass => GrassMaterial,
                TileType.Forest => ForestMaterial,
                TileType.Desert => DesertMaterial,
                TileType.Mountain => MountainMaterial,
                TileType.Water => WaterMaterial,
                _ => HexPrefab.GetComponent<MeshRenderer>().material
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
 
            startPos = new Vector3(x, 0, z);
        }
 
        Vector3 CalculateWorldPosition(Vector2 gridPos)
        {
            float offset = 0;
            if (gridPos.y % 2 != 0)
                offset = hexWidth / 2;
 
            var x = startPos.x + gridPos.x * hexWidth + offset;
            var z = startPos.z - gridPos.y * hexHeight * 0.75f;
 
            return new Vector3(x, 0, z);
        }
    }
}
