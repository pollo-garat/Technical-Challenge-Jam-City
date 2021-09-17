using UnityEngine;

namespace Pathfinding.Scripts.Gameplay.Domain.Views
{
    public class CreateWorldGrid : MonoBehaviour 
    {
        public Transform hexPrefab;
 
        public int gridWidth = 11;
        public int gridHeight = 11;
 
        float hexWidth = 1f;
        float hexHeight = 1f;
        public float gap = 0.0f;
 
        Vector3 startPos;
 
        public void Create()
        {
            AddGap();
            CalculateStartPosition();
            CreateGrid();
        }
 
        void AddGap()
        {
            hexWidth += hexWidth * gap;
            hexHeight += hexHeight * gap;
        }
 
        void CalculateStartPosition()
        {
            float offset = 0;
            if (gridHeight / 2 % 2 != 0)
                offset = hexWidth / 2;
 
            var x = -hexWidth * (gridWidth / 2) - offset;
            var z = hexHeight * 0.75f * (gridHeight / 2);
 
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

        void CreateGrid()
        {
            for (var y = 0; y < gridHeight; y++)
            {
                for (var x = 0; x < gridWidth; x++)
                {
                    var hex = Instantiate(hexPrefab);
                    var gridPos = new Vector2(x, y);
                    hex.position = CalculateWorldPosition(gridPos);
                    hex.parent = transform;
                    hex.name = "Hexagon" + x + "|" + y;
                }
            }
        }
    }
}
