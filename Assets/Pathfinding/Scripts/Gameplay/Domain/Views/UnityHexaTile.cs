using System;
using Pathfinding.Scripts.Gameplay.Domain.ValueObjects;
using UniRx;
using UnityEngine;

namespace Pathfinding.Scripts.Gameplay.Domain.Views
{
    public class UnityHexaTile : MonoBehaviour
    {
        public IObservable<HexaTile> OnTileClicked => onTileClicked;

        HexaTile hexaTile;
        readonly ISubject<HexaTile> onTileClicked = new Subject<HexaTile>();

        public void Populate(HexaTile hexaTile) => 
            this.hexaTile = hexaTile;

        void OnMouseDown() => onTileClicked.OnNext(hexaTile);
    }
}
