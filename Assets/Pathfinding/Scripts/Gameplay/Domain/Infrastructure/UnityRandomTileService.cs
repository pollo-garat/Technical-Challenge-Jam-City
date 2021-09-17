using Pathfinding.Scripts.Gameplay.Domain.Services;
using Pathfinding.Scripts.Gameplay.Domain.ValueObjects;

namespace Pathfinding.Scripts.Gameplay.Domain.Infrastructure
{
    public class UnityRandomTileService : RandomTileService
    {
        readonly HexaTileConfiguration grassConfiguration;
        readonly HexaTileConfiguration forestConfiguration;
        readonly HexaTileConfiguration desertConfiguration;
        readonly HexaTileConfiguration mountainConfiguration;
        readonly HexaTileConfiguration waterConfiguration;

        public UnityRandomTileService(
            HexaTileConfiguration grassConfiguration,
            HexaTileConfiguration forestConfiguration,
            HexaTileConfiguration desertConfiguration,
            HexaTileConfiguration mountainConfiguration,
            HexaTileConfiguration waterConfiguration
        )
        {
            this.grassConfiguration = grassConfiguration;
            this.forestConfiguration = forestConfiguration;
            this.desertConfiguration = desertConfiguration;
            this.mountainConfiguration = mountainConfiguration;
            this.waterConfiguration = waterConfiguration;
        }
        
        static readonly System.Random random = new System.Random();
        
        public HexaTile PickOne() => new HexaTile(GetRandomConfiguration());

        HexaTileConfiguration GetRandomConfiguration() =>
            random.Next(1, 6) switch
            {
                5 => waterConfiguration,
                4 => mountainConfiguration,
                3 => desertConfiguration,
                2 => forestConfiguration,
                1 => grassConfiguration,
                _ => grassConfiguration
            };
    }
}