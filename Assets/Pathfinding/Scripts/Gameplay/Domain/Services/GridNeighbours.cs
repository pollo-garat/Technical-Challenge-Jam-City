using System.Collections.Generic;
using System.Linq;
using Pathfinding.Scripts.Gameplay.Domain.ValueObjects;

namespace Pathfinding.Scripts.Gameplay.Domain.Services
{
    public class GridNeighbours
    {
        readonly int MaxX;
        readonly int MaxYEven;
        readonly int MaxYOdd;
        
        public GridNeighbours(int gridWidth, int gridHeight)
        {
            MaxX = gridWidth;
            MaxYEven = gridHeight;
            MaxYOdd = gridHeight;
        }

        public IEnumerable<(int, int)> ValidNeighbours(HexaTile hexaTile) =>
            NeighboursFrom(hexaTile)
                .Select(pair => (hexaTile.X + pair.y, hexaTile.Y + pair.x))
                .Where(IsValid);

        bool IsValid((int x, int y) tile) => 
            tile.x >= 0 &&
            tile.y >= 0 &&
            tile.x < MaxX &&
            tile.y < (IsEven(tile.y) ? MaxYEven : MaxYOdd);
        
        static IEnumerable<(int x, int y)> NeighboursFrom(HexaTile hexaTile) => 
            IsEven(hexaTile.Y) ? EvenNeighbours : OddNeighbours;

        static IEnumerable<(int x, int y)> EvenNeighbours => 
            new[] { Left, BottomLeftEven, Right, BottomRightEven, TopLeftEven, TopRightEven };

        static IEnumerable<(int x, int y)> OddNeighbours => 
            new[] { Left, BottomLeftOdd, Right, BottomRightOdd, TopLeftOdd, TopRightOdd };
        
        static bool IsEven(int row) => row % 2 == 0;

        static (int x, int y) Right => (0, 1);
        static (int x, int y) BottomRightEven => (1, 0);
        static (int x, int y) BottomRightOdd => (1, 1);
        static (int x, int y) Left => (0, -1);
        static (int x, int y) BottomLeftEven => (1, -1);
        static (int x, int y) BottomLeftOdd => (1, 0);
        static (int x, int y) TopLeftOdd => (-1, 0);
        static (int x, int y) TopRightOdd => (-1, 1);
        static (int x, int y) TopRightEven => (-1, 0);
        static (int x, int y) TopLeftEven => (-1, -1);
    }
}