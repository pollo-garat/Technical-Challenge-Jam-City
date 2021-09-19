using System;
using System.Collections.Generic;
using System.Linq;
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
        public Material NeighbourMaterial;

        public bool DebugMode;
        
        float hexWidth = 1f;
        float hexHeight = 1f;
 
        Vector3 startPosition;
        HexaTile[,] grid;
        UnityHexaTile[,] unityGird;
        GridNeighbours girdNeighbours;
        
        List<UnityHexaTile> openList = new List<UnityHexaTile>();
        List<UnityHexaTile> closeList = new List<UnityHexaTile>();

        public void CreateGrid(HexaTile[,] grid, GridNeighbours girdNeighbours)
        {
            this.grid = grid;
            this.girdNeighbours = girdNeighbours;
            unityGird = new UnityHexaTile[GridWidth, GridHeight];
            AddGap();
            CalculateStartPosition();
            Create();
        }

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

        public void PaintNeighbours(IEnumerable<(int, int)> neighbours)
        {
            foreach (var unityTile in unityGird) 
                unityTile.ResetMaterial();

            foreach (var neighbour in neighbours)
            {
                unityGird[neighbour.Item1, neighbour.Item2]
                    .GetComponent<UnityHexaTile>().SetNewMaterial(NeighbourMaterial);
            }
        }

        public void FindPath(IEnumerable<HexaTile> selectedTiles)
        {
            if (!StartAndEndTileAre(selectedTiles)) return;
            
            var startingTile = selectedTiles.First();
            var endTile = selectedTiles.Last();

            var startNode = GetObjectFromGrid(startingTile);
            var endNode = GetObjectFromGrid(endTile);
                
            openList = new List<UnityHexaTile> { startNode };
            closeList = new List<UnityHexaTile>();

            foreach (var tile in unityGird)
            {
                tile.GCost = int.MaxValue;
                tile.CalculateFCost();
                tile.CameFromNode = null;
            }
                
            startNode.GCost = 0;
            startNode.HCost = CalculateDistanceCost(startNode, endNode);
            startNode.CalculateFCost();

            while (openList.Count > 0)
            {
                var currentNode = GetLowestFCost(openList);

                if (currentNode == endNode)
                {
                    foreach (var tile in RetrievePath(endNode)) 
                        tile.SetNewMaterial(NeighbourMaterial);
                    return;
                }

                openList.Remove(currentNode);
                closeList.Add(currentNode);

                foreach (var neighbourNode in GetNeighbours(currentNode))
                {
                    if(closeList.Contains(neighbourNode))
                        continue;

                    var tentativeGCost = currentNode.GCost + CalculateDistanceCost(currentNode, neighbourNode);
                    
                    if (tentativeGCost < neighbourNode.GCost && neighbourNode.HexaTile.Configuration.IsWalkable)
                    {
                        neighbourNode.CameFromNode = currentNode;
                        neighbourNode.GCost = tentativeGCost;
                        neighbourNode.HCost = CalculateDistanceCost(neighbourNode, endNode);
                        neighbourNode.CalculateFCost();
                            
                        if(!openList.Contains(neighbourNode))
                            openList.Add(neighbourNode);
                    }
                }
            }
        }

        static int CalculateDistanceCost(UnityHexaTile startNode, UnityHexaTile endNode)
        {
            var xDistance = Mathf.Abs(startNode.HexaTile.X - endNode.HexaTile.X);
            var yDistance = Mathf.Abs(startNode.HexaTile.Y - endNode.HexaTile.Y);
            var remaining = Mathf.Abs(xDistance - yDistance);

            return 10 * Mathf.Min(xDistance, yDistance) + 14 * remaining;
        }

        static IEnumerable<UnityHexaTile> RetrievePath(UnityHexaTile endNode)
        {
            var path = new List<UnityHexaTile> {endNode};
            var currentNode = endNode;
            
            while (currentNode.CameFromNode != null)
            {
                path.Add(currentNode.CameFromNode);
                currentNode = currentNode.CameFromNode;
            }

            path.Reverse();
            return path;
        }

        IEnumerable<UnityHexaTile> GetNeighbours(UnityHexaTile tile) => 
            girdNeighbours.ValidNeighbours(tile.HexaTile)
                .Select(neighbour => GetObjectFromGrid(neighbour));

        static UnityHexaTile GetLowestFCost(IReadOnlyList<UnityHexaTile> readyForSearchTiles)
        {
            var lowestFCost = readyForSearchTiles[0];

            for (var i = 1; i < readyForSearchTiles.Count; i++)
            {
                if (readyForSearchTiles[i].FCost < lowestFCost.FCost) 
                    lowestFCost = readyForSearchTiles[i];
            }

            return lowestFCost;
        }
        
        UnityHexaTile GetObjectFromGrid(HexaTile tile) => 
            unityGird[tile.X, tile.Y];
        
        UnityHexaTile GetObjectFromGrid((int x, int y) coordinates) => 
            unityGird[coordinates.x, coordinates.y];

        static bool StartAndEndTileAre(IEnumerable<HexaTile> selectedTiles) => 
            selectedTiles.Count() > 1;

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
