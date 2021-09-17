using Pathfinding.Scripts.Gameplay.Domain.ValueObjects;
using UnityEngine;

namespace Pathfinding.Scripts.Gameplay.Domain.Views
{
    public class UnityHexaTile : MonoBehaviour
    {
        HexaTileConfiguration hexaTileConfiguration;
        
        public void SaveConfiguration(HexaTileConfiguration hexaTileConfiguration)
        {
            this.hexaTileConfiguration = hexaTileConfiguration;
        }

        void OnMouseDown()
        {
            Debug.Log($"{hexaTileConfiguration.ToString()}");
        }
    }
}
