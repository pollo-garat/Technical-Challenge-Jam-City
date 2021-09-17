using System;
using Pathfinding.Scripts.Gameplay.Domain.ValueObjects;
using UniRx;
using UnityEngine;

namespace Pathfinding.Scripts.Gameplay.Domain.Views
{
    public class UnityHexaTile : MonoBehaviour
    {
        public IObservable<HexaTileConfiguration> OnTileClicked => onTileClicked;

        HexaTileConfiguration hexaTileConfiguration;
        readonly ISubject<HexaTileConfiguration> onTileClicked = new Subject<HexaTileConfiguration>();

        public void SaveConfiguration(HexaTileConfiguration hexaTileConfiguration)
        {
            this.hexaTileConfiguration = hexaTileConfiguration;
        }

        void OnMouseDown() => onTileClicked.OnNext(hexaTileConfiguration);
    }
}
