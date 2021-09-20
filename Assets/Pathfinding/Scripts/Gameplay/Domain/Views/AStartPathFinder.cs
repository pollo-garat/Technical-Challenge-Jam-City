using System.Collections.Generic;
using System.Linq;
using Pathfinding.Scripts.Gameplay.Domain.Services;
using Pathfinding.Scripts.Gameplay.Domain.ValueObjects;
using UnityEngine;

namespace Pathfinding.Scripts.Gameplay.Domain.Views
{
    public class AStartPathFinder : MonoBehaviour
    {
        public Material PathMaterial;
        public Material HighlightMaterial;
        public Material NeighbourMaterial;

        List<UnityHexaTile> openList = new List<UnityHexaTile>();
        List<UnityHexaTile> closeList = new List<UnityHexaTile>();

        UnityHexaTile[,] unityGird;
        GridNeighbours girdNeighbours;
        
        public void Initialize(UnityHexaTile[,] unityGird, GridNeighbours girdNeighbours)
        {
            this.unityGird = unityGird;
            this.girdNeighbours = girdNeighbours;
        }
        
        public void FindPath(IEnumerable<HexaTile> selectedTiles)
        {
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
                    var path = RetrievePath(endNode);

                    foreach (var tile in path.Skip(1).Take(path.Count() - 2)) 
                        tile.SetNewMaterial(PathMaterial);
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
        
        public void PaintNeighbours(IEnumerable<(int, int)> neighbours)
        {
            ResetMapGraphics();

            foreach (var neighbour in neighbours)
            {
                unityGird[neighbour.Item1, neighbour.Item2]
                    .GetComponent<UnityHexaTile>().SetNewMaterial(NeighbourMaterial);
            }
        }
        
        public void HighlightTile(IEnumerable<HexaTile> tiles)
        {
            foreach (var tile in tiles)
                GetObjectFromGrid(tile).SetNewMaterial(HighlightMaterial);
        }
        
        public void ResetMapGraphics()
        {
            foreach (var unityTile in unityGird) 
                unityTile.ResetMaterial(); 
        }
        
        static int CalculateDistanceCost(UnityHexaTile startNode, UnityHexaTile endNode)
        {
            var xDistance = Mathf.Abs(startNode.HexaTile.X - endNode.HexaTile.X);
            var yDistance = Mathf.Abs(startNode.HexaTile.Y - endNode.HexaTile.Y);
            var remaining = Mathf.Abs(xDistance - yDistance);

            return startNode.GetCost * 10 * Mathf.Min(xDistance, yDistance) + endNode.GetCost * 14 * remaining;
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
        
        UnityHexaTile GetObjectFromGrid((int x, int y) coordinates) => 
            unityGird[coordinates.x, coordinates.y];

        UnityHexaTile GetObjectFromGrid(HexaTile tile) => 
            unityGird[tile.X, tile.Y];
    }
}