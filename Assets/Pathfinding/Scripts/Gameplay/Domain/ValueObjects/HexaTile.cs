namespace Pathfinding.Scripts.Gameplay.Domain.ValueObjects
{
    public struct HexaTile
    {
        public HexaTileConfiguration Configuration;
        public int X;
        public int Y;
        
        public HexaTile(HexaTileConfiguration configuration, int x, int y)
        {
            Configuration = configuration;
            X = x;
            Y = y;
        }

        public HexaTile(HexaTileConfiguration configuration)
        {
            Configuration = configuration;
            X = 0;
            Y = 0;
        }

        public static HexaTile SetTileInCoordinates(HexaTile tile, int x, int y) => 
            new HexaTile(tile.Configuration, x, y);

        public TileType Type() => Configuration.Type;

        public override string ToString() =>
            $"Configuration: {Configuration.ToString()} Coordinate X: {X}, Coordinate Y: {Y}";
        
        public override bool Equals(object obj) =>
            obj is HexaTile other && Equals(other);
        
        bool Equals(HexaTile other) => 
            X == other.X &&
            Y == other.Y;
    }
}